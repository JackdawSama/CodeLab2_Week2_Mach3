using UnityEngine;
using System.Collections;

public class RepopulateScript : MonoBehaviour 
{
	
	protected GameManagerScript gameManager;

	public virtual void Start() 
	{
		gameManager = GetComponent<GameManagerScript>();
	}

	//adds new tokens into empty spaces
	public virtual void AddNewTokensToRepopulateGrid()
	{
		//checks the top line of the grid for an empty
		for(int x = 0; x < gameManager.gridWidth; x++)
		{
			//if there is no token in a space
			GameObject token = gameManager.gridArray[x, gameManager.gridHeight - 1];
			if(token == null)
			{
				//adds token to empty space
				gameManager.AddTokenToPosInGrid(x, gameManager.gridHeight - 1, gameManager.grid);
			}
		}
	}
}
