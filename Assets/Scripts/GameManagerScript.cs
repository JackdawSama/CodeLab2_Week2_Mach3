using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour 
{

	public int gridWidth = 8;
	public int gridHeight = 8;
	public float tokenSize = 1;

	protected MatchManagerScript matchManager;
	protected InputManagerScript inputManager;
	protected RepopulateScript repopulateManager;
	protected MoveTokensScript moveTokenManager;

	public GameObject grid;
	public  GameObject[,] gridArray;
	protected Object[] tokenTypes;
	GameObject selected;

	//declaring/instantiating the tokens, the grid, and the managers
	public virtual void Start () 
	{
		tokenTypes = (Object[])Resources.LoadAll("Tokens/"); //loads all token types
		gridArray = new GameObject[gridWidth, gridHeight]; //instantiates the grid
		MakeGrid(); //runs MakeGrid
		matchManager = GetComponent<MatchManagerScript>(); //get managers
		inputManager = GetComponent<InputManagerScript>();
		repopulateManager = GetComponent<RepopulateScript>();
		moveTokenManager = GetComponent<MoveTokensScript>();
	}

	//checking grid
	public virtual void Update()
	{
		//if grid is has no empties
		if(!GridHasEmpty())
		{
			//if any matches, remove them
			if(matchManager.GridHasMatch())
			{
				matchManager.RemoveMatches();
			} else //otherwise, select token
			{
				inputManager.SelectToken();
			}
		} else //if grid has empties
		{
			//if moveTokenManger is not moving, start TokenMove
			if(!moveTokenManager.move)
			{
				moveTokenManager.SetupTokenMove();
			}
			//if not moving tokens to fill empties, add new token
			if(!moveTokenManager.MoveTokensToFillEmptySpaces())
			{
				repopulateManager.AddNewTokensToRepopulateGrid();
			}
		}
	}

	//make the grid
	void MakeGrid() 
	{
		//create new grid game object
		grid = new GameObject("TokenGrid");
		//creates grid of certain size and adds tokens to grid
		for(int x = 0; x < gridWidth; x++)
		{
			for(int y = 0; y < gridHeight; y++)
			{
				AddTokenToPosInGrid(x, y, grid);
			}
		}
	}

	//if the grid has any empties
	public virtual bool GridHasEmpty(){
		//checks each space in the grid, if any returns null, set this to true, else, false
		for(int x = 0; x < gridWidth; x++)
		{
			for(int y = 0; y < gridHeight ; y++)
			{
				if(gridArray[x, y] == null)
				{
					return true;
				}
			}
		}

		return false;
	}

	//returns the position of the selected token in the grid
	public Vector2 GetPositionOfTokenInGrid(GameObject token)
	{
		for(int x = 0; x < gridWidth; x++)
		{
			for(int y = 0; y < gridHeight ; y++)
			{
				//if there is a token, return the position
				if(gridArray[x, y] == token)
				{
					return(new Vector2(x, y));
				}
			}
		}
		return new Vector2();
	}
		
	//gets the world position of the object based on the grid position
	public Vector2 GetWorldPositionFromGridPosition(int x, int y)
	{
		//finds the center of the token and returns as position
		return new Vector2(
			(x - gridWidth/2) * tokenSize,
			(y - gridHeight/2) * tokenSize);
	}

	//adds token to specific location on grid
	public void AddTokenToPosInGrid(int x, int y, GameObject parent)
	{
		//takes the world position and instantiates a token as game object in world position
		Vector3 position = GetWorldPositionFromGridPosition(x, y);
		GameObject token = 
			Instantiate(tokenTypes[Random.Range(0, tokenTypes.Length)], 
			            position, 
			            Quaternion.identity) as GameObject;
		token.transform.parent = parent.transform;
		gridArray[x, y] = token;
	}
}
