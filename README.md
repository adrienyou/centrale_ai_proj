VampiresVSWerwolves project

## Last commit description 
	J'ai rajouté un module Tree qui contient deux classes : 
		- TreeNode, qui comme vous l'avez compris représente un noeud de l'arbre. 
		Trois fields : le noeud parent, la liste des noeuds enfants et la valeur.
		Après avoir vérifié, pas besoin de pointers car en C#, les fields sont passés 
		par référence (donc on ne recréer pas un object)

		- TreeNodeList, qui est une liste de TreeNode, avec un seul field, le parent. 
		Contient une méthode add, qui rajoute un noeud à une liste, égalisant son parent 
		au parent de la liste.

## Previous commits description
	2) Added Map class
	1) First commit
