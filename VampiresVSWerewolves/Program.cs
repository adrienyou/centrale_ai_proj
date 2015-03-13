using System;
using System.Collections.Generic;
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

            int width = Convert.ToInt16(buffer[4]); // Nombre de colonnes
            int height = Convert.ToInt16(buffer[3]); // Nombre de lignes
            Map map = new Map(height, width);

            Console.WriteLine("Row number: " + Convert.ToString(map.Height));
            Console.WriteLine("Column number: " + Convert.ToString(map.Width));

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

            //On recoit HME, c'est-à-dire la case où sont placés vos monstres
            while (socket.Available < 5)
                Thread.Sleep(100);
            socket.Receive(buffer, 5, SocketFlags.Partial);
            if (Encoding.ASCII.GetString(buffer, 0, 3) != "HME")
                throw new Exception("Erreur, attendu: HME");

            //Mettez à jour votre grille en utilisant (buffer[3], buffer[4]) comme coordonnées de votre demeure
            Console.WriteLine("Ma demeure: " + Convert.ToString(buffer[3]) + " & " + Convert.ToString(buffer[4]));

            //On recoit MAP
            while (socket.Available < 4)
                Thread.Sleep(100);
            socket.Receive(buffer, 4, SocketFlags.Partial);
            if (Encoding.ASCII.GetString(buffer, 0, 3) != "MAP")
                throw new Exception("Erreur, attendu: MAP");
            while (socket.Available < buffer[3] * 5)
                Thread.Sleep(100);

            read = socket.Receive(buffer, buffer[3] * 5, SocketFlags.Partial);

            //Read contient 5x le nombre de 5-tuplets.
            //Buffer contient la liste des changements
            Console.WriteLine("READ: " + Convert.ToString(read));

            State state = new State(map);
            state.Update(read, buffer);

            Position pos = new Position(5, 4);
            string pos_str = pos.Stringify();
            Cell cell = (Cell)state.Cells[pos_str];
            Console.WriteLine("TEST STATE: " + Convert.ToString(cell.Pop));
            Console.WriteLine("TEST STRING: " + state.getKey());
            

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
                }

                //ICI FAITES VOS CALCULS
                byte[] response = new byte[0];
                //créez un byte[] contenant tout ce qu'il faut
                socket.Send(response);
            }

            socket.Close();
            socket.Dispose();
        }
    }
}
