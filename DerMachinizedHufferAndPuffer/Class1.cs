using System;

namespace DerMachinizedHufferAndPuffer
{
	public class Node
	{

		unsafe public Node(int chance, Node* parent, Node* left, Node* right, char* symb = NULL) :
	_chance(chance),
	_parent(parent),
	_left(left),
	_right(right),
	_symb(symb)
		{
		}

		public int getChance() { return _chance; }
		public void setParent(Node* parent) { _parent = parent; }
		public char* getSymb() { return _symb; }
		public Node* getLeft() { return _left; }
		public Node* getRight() { return _right; }
		public Node* getParent() { return _parent; }
		public void steps(Node* startNode);
		public void addCodeSymb(Node* target, char symb);
		public Node* getR() { return _right; }
		public Node* getL() { return _left; }

		unsafe private char* _symb;
		private int _chance; //byte val
		unsafe private Node* _parent;
		unsafe private Node* _left;
		unsafe private Node* _right;
	};


	class Tree
	{
		public:
	Tree(char* uniqueSymbArr, int uniqueSymbols, int* chances);
		void genTree();
		void deGenTree();
		int getLessChanceNode(Node*** nodesfield, Node** node1, Node** node2);
		bool checkGenSuc(Node*** nodesfield);
		void steps(Node* startNode);
		void addCodeSymb(Node* target, char symb);
		uint64_t* getCodes() { return _codes; }
		void setNodeCounter(int newCounter) { _currentNodeCounter = newCounter; }
		int getNodeCounter() { return _currentNodeCounter; }
		void coutNodeInfo(Node* target);
		void showTree(Node*** nodes, int nodesAm);

		int getNodeInd(Node** nodes, Node* target)
		{
			for (uint64_t i = 0; i < this->getNodeCounter(); i++)
				if (nodes[i] == target)
					return i;
		}
		private:
	Node* _root;
		int* _chances; //arr
		uint64_t* _codes; //arr
		int _uniqueSymbAm;
		char* _uniqueSymb; //arr
		int _currentNodeCounter;
		Node** nodes;
	};

	Tree::Tree(char* uniqueSymbArr, int uniqueSymbols, int* chances) :
	_uniqueSymb(uniqueSymbArr),
	_uniqueSymbAm(uniqueSymbols),
	_chances(chances)
	{
		_currentNodeCounter = _uniqueSymbAm;
	}

	void Tree::coutNodeInfo(Node* target)
	{
		cout << "[*******]\n" << "Node addr: " << target << " + 0 addr: " << (*nodes) << endl
			<< "Node number: " << target - (*nodes) << endl;
	}

	void Tree::genTree()
	{
		//mem 4 nodes
		nodes = (Node**)malloc(sizeof(Node*) * _uniqueSymbAm);
		for (uint64_t i = 0; i < _uniqueSymbAm; i++)
			nodes[i] = new Node(_chances[i], NULL, NULL, NULL, _uniqueSymb + i);
		Node* n1, *n2;
		while (!checkGenSuc(&nodes))
		{
			getLessChanceNode(&nodes, &n2, &n1);
			//cout << sizeof(nodes)/sizeof(Node*) << endl;
			//cout << sizeof(nodes) << endl;
			this->setNodeCounter(this->getNodeCounter() + 1);
			nodes = (Node**)realloc(nodes, sizeof(Node*) * this->getNodeCounter());
			nodes[this->getNodeCounter() - 1] = new Node(n1->getChance() + n2->getChance(), NULL, n2, n1);
			n1->setParent(nodes[this->getNodeCounter() - 1]);
			n2->setParent(nodes[this->getNodeCounter() - 1]);
			cout << "[*******]\n\tNEw node nuMber " << this->getNodeCounter() - 1 <<
				"\n\twith node ChAnce " << nodes[this->getNodeCounter() - 1]->getChance() <<
				"\n\tWas gened from nodes: " << this->getNodeInd(nodes, n1) << "(" << n1->getChance() << ") and " << this->getNodeInd(nodes, n2) << "(" << n2->getChance() << ")" << endl;
		}

		cout << "calling less chance for root: " << endl;
		getLessChanceNode(&nodes, &n2, &n1);
		_root = new Node(n1->getChance() + n2->getChance(), NULL, n2, n1);
		n1->setParent(_root);
		n2->setParent(_root);
		this->showTree(&nodes, this->getNodeCounter());
	}

	void Tree::deGenTree()
	{
		_codes = new uint64_t[_uniqueSymbAm];
		for (uint64_t i = 0; i < _uniqueSymbAm; i++)
			_codes[i] = 0;
		addCodeSymb(_root, 1);
		steps(_root);
	}

	void Tree::steps(Node* startNode)
	{
		if (startNode->getR() != NULL)
			addCodeSymb(startNode->getR(), 1);
		if (startNode->getL() != NULL)
			addCodeSymb(startNode->getL(), 0);
		if (startNode->getR() != NULL)
			steps(startNode->getR());
		if (startNode->getL() != NULL)
			steps(startNode->getL());
	}

	void Tree::addCodeSymb(Node* target, char symb)
	{
		if (target->getSymb() != NULL)
		{
			//cout << "WHat rhe HEll Is ThiS : " << ((target->getSymb() - _uniqueSymb)) << endl;
			_codes[(target->getSymb() - _uniqueSymb)] = _codes[(target->getSymb() - _uniqueSymb)] << 1;//not sure
			_codes[(target->getSymb() - _uniqueSymb)] += symb;
		}
		else
		{
			addCodeSymb(target->getL(), symb);
			addCodeSymb(target->getR(), symb);
		}
	}

	int Tree::getLessChanceNode(Node*** nodesfield, Node** node2, Node** node1)
	{
		//first -> n1
		//n1 =< n2
		int numb1 = 0, numb2 = 0;
		Node* min1 = NULL;
		Node* min2 = NULL;
		for (uint64_t i = 0; i < this->getNodeCounter(); i++)
		{
			if ((*nodesfield)[i]->getParent() == NULL)
			{
				min1 = (*nodesfield)[i];
				numb1 = i;
			}
		}
		for (uint64_t i = 0; i < this->getNodeCounter(); i++)
		{
			if (((*nodesfield)[i]->getParent() == NULL) && (min1 != (*nodesfield)[i]))
			{
				min2 = (*nodesfield)[i];
				numb2 = i;
			}
		}

		//cout << "TEmp: " << "\n\t--> Numb1 : " << numb1 << "\n\t--> numb2: " << numb2 << endl;

		if (min2->getChance() < min1->getChance())
		{
			Node* tBuff = min1;
			min1 = min2;
			min2 = tBuff;
		}
		for (uint64_t i = 0; i < this->getNodeCounter(); i++)
		{
			//this->coutNodeInfo((*nodesfield)[i]);
			if ((*nodesfield)[i]->getParent() == NULL)
			{
				if ((*nodesfield)[i]->getChance() < min1->getChance())
				{
					min2 = min1;
					min1 = (*nodesfield)[i];
					numb2 = numb1; //remove
					numb1 = i;
				}
				else if (((*nodesfield)[i]->getChance() < min2->getChance()) && (min1 != (*nodesfield)[i]))
				{
					min2 = (*nodesfield)[i];
					numb2 = i; //kebab
				}
			}
		}
		*node1 = min1;
		*node2 = min2;
		//cout << "Current \t\\/\n\tmin1 : " << min1->getChance() << "\n\tmin2 : " << min2->getChance() << endl;
		//cout << "\n**************\n\t--> Numb1 : " << numb1 << "\n\t--> numb2: " << numb2 << endl;
		return 0;
	}

	void Tree::showTree(Node*** nodes, int nodesAm)
	{
		cout << "Begin showing tree" << endl;
		for (uint64_t i = 0; i < nodesAm; i++)
		{
			//if (((*nodes)[i]->getSymb() != NULL) && ((*nodes)[i]->getParent() != NULL))
			//cout << "Node number " << i << " : " << (*(*nodes)[i]->getSymb()) << endl;
			if ((*nodes)[i]->getParent() == NULL)
				cout << "No parent node number " << i << endl;
		}
		cout << "End showing tree" << endl;
	}

	bool Tree::checkGenSuc(Node*** nodesfield)
	{
		int counter = 0;
		//int until = sizeof((*(*nodesfield)))/sizeof(Node*); //-> counter
		for (uint64_t i = 0; i < this->getNodeCounter(); i++)
			if ((*nodesfield)[i]->getParent() == NULL)
				counter++;
		return counter == 2 ? true : false;
	}
}
