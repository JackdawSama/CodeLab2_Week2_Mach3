using UnityEngine;
using System.Collections;

public class MatchManagerScript : MonoBehaviour 
{

	protected GameManagerScript gameManager;

	public virtual void Start() 
	{
		gameManager = GetComponent<GameManagerScript>();
	}

	//if there is a match
	public virtual bool GridHasMatch()
	{
		//starts match at false
		bool match = false;
		
		//confusing! *************
		for(int x = 0; x < gameManager.gridWidth; x++)
		{
			for(int y = 0; y < gameManager.gridHeight ; y++)
			{
				//if the selected token is able to form a horizonal match to its right
				if(x < gameManager.gridWidth - 2)
				{
					//only sets to false if both match and HorizontalMatch are false, otherwise true
					match = match || GridHasHorizontalMatch(x, y);
				}

				//NEW BUG FIX
				if(y < gameManager.gridHeight - 2)
				{
					match = match || GridHasVerticalMatch(x,y);
;				}
			}
		}

		return match;
	}

	//checks current tokens and their sprites against eachother
	public bool GridHasHorizontalMatch(int x, int y)
	{
		GameObject token1 = gameManager.gridArray[x + 0, y];
		GameObject token2 = gameManager.gridArray[x + 1, y];
		GameObject token3 = gameManager.gridArray[x + 2, y];
		
		if(token1 != null && token2 != null && token3 != null)
		{
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();
			
			//checks if sprites 1-3 are the same sprite, bool is true
			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
		} else 
		{
			//otherwise stays false
			return false;
		}
	}

	//gets the length of the match (looking for 3)
	public int GetHorizontalMatchLength(int x, int y)
	{
		//starts at 1, for a single token
		int matchLength = 1;
		
		//postion of first token
		GameObject first = gameManager.gridArray[x, y];

		//if first token is not null
		if(first != null)
		{
			//get the sprite of first token
			SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
			
			//checks the token after the first token !!!!!!!!!!!!!!bug?
			for(int i = x + 1; i < gameManager.gridWidth; i++)
			{
				GameObject other = gameManager.gridArray[i, y];

				if(other != null)
				{
					SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

					if(sr1.sprite == sr2.sprite)
					{
						matchLength++;
					} else 
					{
						break;
					}
				} else 
				{
					break;
				}
			}
		}
		
		return matchLength;
	}

	//NEW BUG FIX
	//checks current tokens and their sprites against eachother
	public bool GridHasVerticalMatch(int x, int y)
	{
		GameObject token1 = gameManager.gridArray[x, y + 0];
		GameObject token2 = gameManager.gridArray[x, y + 1];
		GameObject token3 = gameManager.gridArray[x, y + 2];
		
		if(token1 != null && token2 != null && token3 != null)
		{
			SpriteRenderer sr1 = token1.GetComponent<SpriteRenderer>();
			SpriteRenderer sr2 = token2.GetComponent<SpriteRenderer>();
			SpriteRenderer sr3 = token3.GetComponent<SpriteRenderer>();
			
			//checks if sprites 1-3 are the same sprite, bool is true
			return (sr1.sprite == sr2.sprite && sr2.sprite == sr3.sprite);
		} else 
		{
			//otherwise stays false
			return false;
		}
	}

	//NEW BUG FIX
	//gets the length of the match (looking for 3)
	public int GetVerticalMatchLength(int x, int y)
	{
		//starts at 1, for a single token
		int matchLength = 1;
		
		//postion of first token
		GameObject first = gameManager.gridArray[x, y];

		//if first token is not null
		if(first != null)
		{
			//get the sprite of first token
			SpriteRenderer sr1 = first.GetComponent<SpriteRenderer>();
			
			//checks the token after the first token !!!!!!!!!!!!!!bug?
			for(int i = y + 1; i < gameManager.gridWidth; i++)
			{
				GameObject other = gameManager.gridArray[x, i];

				if(other != null)
				{
					SpriteRenderer sr2 = other.GetComponent<SpriteRenderer>();

					if(sr1.sprite == sr2.sprite)
					{
						matchLength++;
					} else 
					{
						break;
					}
				} else 
				{
					break;
				}
			}
		}
		
		return matchLength;
	}
		
	//for removing matched tokens
	public virtual int RemoveMatches()
	{
		//number removed
		int numRemoved = 0;

		//for the spaces in the grid
		for(int x = 0; x < gameManager.gridWidth; x++)
		{
			for(int y = 0; y < gameManager.gridHeight ; y++)
			{
				if(x < gameManager.gridWidth - 2)
				{
					//get the length of the Horizontal match
					int horizonMatchLength = GetHorizontalMatchLength(x, y);

					//if greater than 2
					if(horizonMatchLength > 2)
					{
						//destroy token game objects that are matching
						for(int i = x; i < x + horizonMatchLength; i++)
						{
							GameObject token = gameManager.gridArray[i, y]; 
							Destroy(token);

							//increases the number removed, makes sure that gameManager.gridArray sets
							//those spaces to empty
							gameManager.gridArray[i, y] = null;
							numRemoved++;
						}
					}
				}
				//NEW BUG FIX
				if(y < gameManager.gridHeight - 2)
				{
					//get the length of the Horizontal match
					int vertMatchLength = GetVerticalMatchLength(x, y);

					//if greater than 2
					if(vertMatchLength > 2)
					{
						//destroy token game objects that are matching
						for(int i = y; i < y + vertMatchLength; i++)
						{
							GameObject token = gameManager.gridArray[x, i]; 
							Destroy(token);

							//increases the number removed, makes sure that gameManager.gridArray sets
							//those spaces to empty
							gameManager.gridArray[x, i] = null;
							numRemoved++;
						}
					}
				}
			}
		}
		//returns the number of tokens removed
		return numRemoved;
	}
}
