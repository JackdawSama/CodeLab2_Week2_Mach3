using UnityEngine;
using System.Collections;

public class MoveTokensScript : MonoBehaviour 
{

	protected GameManagerScript gameManager;
	protected MatchManagerScript matchManager;

	public bool move = false;

	public float lerpPercent;
	public float lerpSpeed;

	bool userSwap;

	protected GameObject exchangeToken1;
	GameObject exchangeToken2;

	Vector2 exchangeGridPos1;
	Vector2 exchangeGridPos2;

	public virtual void Start() 
	{
		gameManager = GetComponent<GameManagerScript>();
		matchManager = GetComponent<MatchManagerScript>();
		lerpPercent = 0;
	}

	public virtual void Update() 
	{
		//if move is happening
		if(move)
		{
			//add lerp percent to lerp speed
			lerpPercent += lerpSpeed;

			//if lerp percent is greater than or equal to 1, set to 1
			if(lerpPercent >= 1)
			{
				lerpPercent = 1;
			}

			//if there is a token in exchangeToken1, exchange tokens
			if(exchangeToken1 != null)
			{
				ExchangeTokens();
			}
		}
	}

	//sets move to true and lerp to 0
	public void SetupTokenMove()
	{
		move = true;
		lerpPercent = 0;
	}

	//caches the position of the two tokens that will be exchanged
	public void SetupTokenExchange(GameObject token1, Vector2 pos1,
	                               GameObject token2, Vector2 pos2, bool reversable){
		SetupTokenMove();

		exchangeToken1 = token1;
		exchangeToken2 = token2;

		exchangeGridPos1 = pos1;
		exchangeGridPos2 = pos2;


		this.userSwap = reversable;
	}

	public virtual void ExchangeTokens()
	{

		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos1.x, (int)exchangeGridPos1.y);
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition((int)exchangeGridPos2.x, (int)exchangeGridPos2.y);

//		Vector3 movePos1 = Vector3.Lerp(startPos, endPos, lerpPercent);
//		Vector3 movePos2 = Vector3.Lerp(endPos, startPos, lerpPercent);

		Vector3 movePos1 = SmoothLerp(startPos, endPos, lerpPercent);
		Vector3 movePos2 = SmoothLerp(endPos, startPos, lerpPercent);

		exchangeToken1.transform.position = movePos1;
		exchangeToken2.transform.position = movePos2;

		if(lerpPercent == 1)
		{
			gameManager.gridArray[(int)exchangeGridPos2.x, (int)exchangeGridPos2.y] = exchangeToken1;
			gameManager.gridArray[(int)exchangeGridPos1.x, (int)exchangeGridPos1.y] = exchangeToken2;

			if(!matchManager.GridHasMatch() && userSwap)
			{
				SetupTokenExchange(exchangeToken1, exchangeGridPos2, exchangeToken2, exchangeGridPos1, false);
			} else {
				exchangeToken1 = null;
				exchangeToken2 = null;
				move = false;
			}
		}
	}

	//smooths the lerp for token movement
	private Vector3 SmoothLerp(Vector3 startPos, Vector3 endPos, float lerpPercent)
	{
		return new Vector3(
			Mathf.SmoothStep(startPos.x, endPos.x, lerpPercent),
			Mathf.SmoothStep(startPos.y, endPos.y, lerpPercent),
			Mathf.SmoothStep(startPos.z, endPos.z, lerpPercent));
	}

	//sets a start pos and end pos from world pos for the token
	public virtual void MoveTokenToEmptyPos(int startGridX, int startGridY,
	                                int endGridX, int endGridY,
	                                GameObject token){
	
		Vector3 startPos = gameManager.GetWorldPositionFromGridPosition(startGridX , startGridY);
		Vector3 endPos = gameManager.GetWorldPositionFromGridPosition(endGridX, endGridY);

		//lerps from start pos to end pos based on lerp percent
		Vector3 pos = Vector3.Lerp(startPos, endPos, lerpPercent);

		//updates tokens position to the pos
		token.transform.position =	pos;

		//if lerp has reached 1, sets end pos to be token's pos and start pos to null
		if(lerpPercent == 1)
		{
			gameManager.gridArray[endGridX, endGridY] = token;
			gameManager.gridArray[startGridX, startGridY] = null;
		}
	}

	//move tokens to fill empties
	public virtual bool MoveTokensToFillEmptySpaces()
	{
		//starts moved token at false
		bool movedToken = false;

		//checks grid both directions
		for(int x = 0; x < gameManager.gridWidth; x++)
		{
			for(int y = 1; y < gameManager.gridHeight ; y++)
			{
				//should be checking one space below token for empty (!)
				if(gameManager.gridArray[x, y - 1] == null)
				{
					for(int pos = y; pos < gameManager.gridHeight; pos++)
					{
						//if there is a token above an empty space
						GameObject token = gameManager.gridArray[x, pos];
						if(token != null)
						{
							//move token to the empty space and set moved token to true
							MoveTokenToEmptyPos(x, pos, x, pos - 1, token);
							movedToken = true;
						}
					}
				}
			}
		}

		if(lerpPercent == 1)
		{
			move = false;
		}

		return movedToken;
	}
}
