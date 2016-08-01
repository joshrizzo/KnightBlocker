#include <cstdlib>
#include <ctime>
#include <iostream>
#include <utility>
#include <map>
using namespace std;


typedef struct node {
	int x;
	int y;
	bool bChecked;
	bool bVisited;
	bool bBlocked;
	bool Reachable;
	multimap<double, node *> neighbors;
	inline bool inPlay(void) { return !bBlocked && !bVisited;}
	void Attach(int x, int y, node ** nodeBoard, int xmax, int ymax, int xend, int yend);
	const double D(int u, int v) const;
};

typedef multimap<double, node *> NodeMap;
typedef NodeMap::iterator NodeIter;
int xend = 0;
int yend = 0;
bool Traverse(int xstart, int ystart, int xend, int yend, node ** board);
int main(void)
{
	int finished = 0;
	// Seeds the random number generator.
	srand(time(nullptr));
	// Generate a set of 5000 obstacles on a board with dimensions 1000x1000 randomly.
	int nBoardX = 1000;
	int nBoardY = 1000;

	// Get X and Y dimensions from the user
	cin >> nBoardX >> nBoardY;

	// Initialize a board.
	// Set the boards entirely to false.
	node ** Board = new node*[nBoardX];
	for (int i = 0; i < nBoardX; i++)
	{
		Board[i] = new node[nBoardY];
		for (int j = 0; j < nBoardY; j++)
		{
			Board[i][j].x = i;
			Board[i][j].y = j;
			Board[i][j].bBlocked = false;
			Board[i][j].bVisited = false;
			Board[i][j].Reachable = true;
			Board[i][j].bChecked = false;
		}
	}
	
	
	// Ask the user for a starting position.
	int nI, nJ;
	cin >> nI >> nJ;
	nI--;
	nJ--;
	//nI = rand() % nBoardX;
	//nJ = rand() % nBoardY;

	// Ask the user for a terminating position.
	int nK, nL;
	cin >> nK; nK--;
	cin >> nL; nL--;

	/*do
	{
		nK = rand() % nBoardX;
		nL = rand() % nBoardY;
	} while (nI == nK && nJ == nL);
	*/

	// Ask the user for the number of obstacles;
	int nC = 1;
	cin >> nC;

	int nObstacleCount = 0;
	while (nObstacleCount != nC)
	{
		int nX, nY;
		
		cin >> nX;
		cin >> nY;
		nX--; nY--;
		//nX = rand() % nBoardX;
		//nY = rand() % nBoardY;
		/*if (!Board[nX][nY].bBlocked && (nX != nI && nY != nJ) && (nX != nK && nY != nL))
		{
			Board[nX][nY].bBlocked = true;
			nObstacleCount++;
		}*/
		Board[nX][nY].bBlocked = true;
		nObstacleCount++;
	}
	// Build the board graph here:
	for(int i = 0; i < nBoardX; i++)
		for (int j = 0; j < nBoardY; j++)
		{
			Board[i][j].Attach(i + 1, j + 2, Board, nBoardX, nBoardY, nK, nL);
			Board[i][j].Attach(i + 1, j - 2, Board, nBoardX, nBoardY, nK, nL);
			Board[i][j].Attach(i - 1, j - 2, Board, nBoardX, nBoardY, nK, nL);
			Board[i][j].Attach(i - 1, j + 2, Board, nBoardX, nBoardY, nK, nL);
			Board[i][j].Attach(i + 2, j + 1, Board, nBoardX, nBoardY, nK, nL);
			Board[i][j].Attach(i + 2, j - 1, Board, nBoardX, nBoardY, nK, nL);
			Board[i][j].Attach(i - 2, j - 1, Board, nBoardX, nBoardY, nK, nL);
			Board[i][j].Attach(i - 2, j + 1, Board, nBoardX, nBoardY, nK, nL);

		}
	cerr << "Start Position: (" << nI << "," << nJ << ")." << endl;
	cerr << "End Position: (" << nK << "," << nL << ")." << endl;
	if (Traverse(nI, nJ, nK, nL, Board))
	{
		for (int i = 0; i < nBoardX; i++)
			for (int j = 0; j < nBoardY; j++)
				Board[i][j].bChecked = false;

		if (Traverse(nI, nJ, nK, nL, Board))
			cout << "No" << endl;
		else
			cout << "Yes" << endl;

		finished = 1;
	}
	else
	{
		cout << "Yes";
		finished = 1;
	}

	for(int i = 0; i < nBoardX; i++)
		delete[] Board[i];
	delete[] Board;

	return finished;
}

void node::Attach(int x, int y, node ** nodeBoard, int xmax, int ymax, int xend, int yend)
{
	if (x >= 0 && x < xmax &&
		y >= 0 && y < ymax &&
		nodeBoard[x][y].inPlay())
		this->neighbors.insert(pair<const double, node *>(nodeBoard[x][y].D(xend, yend), &nodeBoard[x][y]));
}
const double node::D(int u, int v) const
{
	double distance = sqrt((this->x - u) ^ 2 + (this->y - v) ^ 2);
	return distance;
}
bool Traverse(int xstart, int ystart, int xend, int yend, node ** board)
{
	node * curr = &board[xstart][ystart];
	node * end = &board[xend][yend];

	if (curr == end)
		return true;

	if (curr->bChecked == false)
	{
		curr->bChecked = true;
		if (curr->neighbors.count(0) != 0)
		{
			for (NodeIter iter = curr->neighbors.begin(); iter != curr->neighbors.end(); iter++)
			{
				node * check = (*iter).second;
				if (check->inPlay() && Traverse(check->x, check->y, xend, yend, board))
				{
					curr->bVisited = true;
					return true;
				}
			}

		}
	}
	return false;
}

