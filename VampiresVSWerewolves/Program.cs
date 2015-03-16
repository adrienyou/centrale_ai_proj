using System;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Configuration;

namespace VampiresVSWerewolves
{
    class Program
    {
        static byte[]
            NME = Encoding.ASCII.GetBytes("NME"),
            ATK = Encoding.ASCII.GetBytes("ATK"),
            MOV = Encoding.ASCII.GetBytes("MOV");

        static void Main(string[] args)
        {
            string ip = ConfigurationManager.AppSettings["testip"]; //"138.195.86.154";
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["testport"]); //5555;
            string team = ConfigurationManager.AppSettings["team"];

            /****************** DEMARRAGE ******************/
            //Crée le point terminal de la connexion
            var ipep = new System.Net.IPEndPoint(IPAddress.Parse(ip), port);

            //Crée une socket pour se connecter au serveur
            Console.WriteLine("Trying to connect");
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Demande de connection
            socket.Connect(ipep);
            Console.WriteLine("Connected");

            /****************** PROTOCOLE: Nom ******************/
            //On envoie NME
            socket.Send(NME);
            socket.Send(new byte[] { 4 });
            socket.Send(Encoding.ASCII.GetBytes(team)); //<-- Remplacez ici par le nom de votre groupe
            Console.WriteLine("Team name sent");

            /****************** PROTOCOLE: Carte ******************/
            //On reçoit SET
            while (socket.Available < 5)
                Thread.Sleep(100);
            byte[] buffer = new byte[2048];
            socket.Receive(buffer, 5, SocketFlags.Partial);

            if (Encoding.ASCII.GetString(buffer, 0, 3) != "SET")
                throw new Exception("Erreur, attendu: SET");

            int mapHeight = Convert.ToInt16(buffer[3]); // Nombre de lignes, correspond à y
            int mapWidth = Convert.ToInt16(buffer[4]); // Nombre de colonnes, correspond à x

            Console.WriteLine("Map Height: " + mapHeight.ToString());
            Console.WriteLine("Map Width: " + mapWidth.ToString());

            Map map = new Map(mapHeight, mapWidth);      

            //On recoit HUM, cad les cases où sont placées les humains
            Console.WriteLine("Receiving HUM...");
            while (socket.Available < 4)
                Thread.Sleep(100);
            socket.Receive(buffer, 4, SocketFlags.Partial);
            if (Encoding.ASCII.GetString(buffer, 0, 3) != "HUM")
                throw new Exception("Erreur, attendu: HUM");
            while (socket.Available < buffer[3] * 2) //ici on attend les arguments
                Thread.Sleep(100);
            int read = socket.Receive(buffer, buffer[3] * 2, SocketFlags.Partial);

            //Read contient deux fois le nombre de maisons dans la carte
            //pour tout i pair ou nul, buffer[i] contient x et buffer[i+1] contient y
            //On doit faire un truc la?

            //On recoit HME, c'est-à-dire la case où sont placés vos monstres
            while (socket.Available < 5)
                Thread.Sleep(100);
            socket.Receive(buffer, 5, SocketFlags.Partial);
            if (Encoding.ASCII.GetString(buffer, 0, 3) != "HME")
                throw new Exception("Erreur, attendu: HME");

            //(buffer[3], buffer[4]) = (x, y) de ma demeure
            Position myHomePosition = new Position(Convert.ToInt16(buffer[3]), Convert.ToInt16(buffer[4]));

            Console.WriteLine("Ma demeure: " + Convert.ToString(myHomePosition.X) + " & " + Convert.ToString(myHomePosition.Y));

            //On recoit MAP
            while (socket.Available < 4)
                Thread.Sleep(100);
            socket.Receive(buffer, 4, SocketFlags.Partial);
            if (Encoding.ASCII.GetString(buffer, 0, 3) != "MAP")
                throw new Exception("Erreur, attendu: MAP");
            while (socket.Available < buffer[3] * 5)
                Thread.Sleep(100);

            read = socket.Receive(buffer, buffer[3] * 5, SocketFlags.Partial);
           
            State state = new State(map);
            state.FirstUpdate(read, buffer, map, myHomePosition);

            Console.WriteLine("Mon type est: " + map.FriendlyType.ToString());

            /*Position pos = new Position(4, 2);
            string pos_str = pos.Stringify();
            Cell cell = (Cell)state.Cells[pos_str];
            // Console.WriteLine("TEST STATE: " + Convert.ToString(cell.Pop));
            Console.WriteLine("TEST STRING: " + state.getKey());
            */
           /* Move move = new Move(1, 1, 1, 2, 2);
            byte[] resConvert = move.convertToOrder();
            Console.WriteLine("TEST MOVE CONVERT: " + resConvert[4]);
            */
            
            Engine engine = new Engine();
            CellType friendlyType = state.Map.FriendlyType;
            TreeNode<State> initialNode = new TreeNode<State>(state, new List<Move>(), NodeState.Max);
            Hashtable tree = new Hashtable();
            tree.Add(state.getKey(), initialNode);
            
            /****************** PARTIE ******************/
            while (true)
            {
                //ATTENTE D'UN MESSAGE DU SERVEUR (UPD OU END)
                while (socket.Available < 3)
                    Thread.Sleep(100);
                socket.Receive(buffer, 3, SocketFlags.Partial);
                string cmd = Encoding.ASCII.GetString(buffer, 0, 3);

                if (cmd == "END") break; //Ici on gère la sortie de la boucle
                if (cmd != "UPD") throw new Exception("Erreur protocole, attendu : UPD");

                //Si on est là c'est qu'on a reçu UPD
                while (socket.Available < 1)
                    Thread.Sleep(100);
                socket.Receive(buffer, 1, SocketFlags.Partial);

                //Buffer[0] contient le nombre de changements à prendre en compte
                if (buffer[0] > 0)
                {
                    Console.WriteLine("Cellules modifiées:");

                    while (socket.Available < buffer[0] * 5)
                        Thread.Sleep(100);

                    read = socket.Receive(buffer, buffer[0] * 5, SocketFlags.Partial);

                    //Buffer contient read = n * 5 5-tuplets
                    //n est le nombre de changements
                    //Modifiez votre grille ici en fonction des changements
                    state.Update(read, buffer);
                }

                //useRandom(socket, engine, state);
                useNotRandom(socket, engine, state, tree);
            }

            socket.Close();
            socket.Dispose();
        }

        public static void useRandom(Socket socket, Engine engine, State state)
        {
            byte[] response = new byte[5];

            List<Move> moves = engine.RandomSuccessor(state);
            Move move = moves[0];

            //Default value
            string startBuffer = "MOV";

/*  En fait, envoyer ATK fait planter, meme si il y a bien un ennemi
 *  sur la case d'arriver.
 *  Donc pas besoin
 * 
 * if (state.Cells.ContainsKey(move.PosTo.Stringify()))
            {
                Cell cell = (Cell)state.Cells[move.PosTo.Stringify()];

                // We have to get the type of the cell we're going to, in order to know what string byte to use ("MOV" or "ATK")
                CellType goToCellType = cell.Type;

                if (friendlyType == CellType.Vampires)
                {
                    if (goToCellType == CellType.Humans || goToCellType == CellType.Werewolves)
                    {
                        startBuffer = "ATK";
                    }
                }
                // Means we are Werewolves
                else
                {
                    if (goToCellType == CellType.Humans || goToCellType == CellType.Vampires)
                    {
                        startBuffer = "ATK";
                    }
                }
            }
*/

            Console.WriteLine("Position From: " + move.PosFrom.Stringify());
            Console.WriteLine("Position To: " + move.PosTo.Stringify());
            Console.WriteLine("Pop: " + move.Pop.ToString());

            response = move.convertToOrder();

            socket.Send(Encoding.ASCII.GetBytes(startBuffer));
            socket.Send(new byte[] { (byte)(response.Length / 5) });
            socket.Send(response);
        } 

        public static void useNotRandom(Socket socket, Engine engine, State state, Hashtable tree)
        {            
            //Calculate moves
            TreeNode<State> currentNode = (TreeNode<State>)tree[state.getKey()];
            Tuple<int, TreeNode<State>> result = engine.AlphaBeta(2, -10000, 10000, tree, currentNode, state.Map.FriendlyType);
            TreeNode<State> nextNode = result.Item2;
            List<Move> moves = nextNode.Moves;

            //Default value
            string startBuffer = "MOV";

            socket.Send(Encoding.ASCII.GetBytes(startBuffer));
            socket.Send(new byte[] { (byte)(moves.Count / 5) });

            byte[] response = Move.convertToByteArray(moves);

            socket.Send(response);
            
        } 
    }
}
