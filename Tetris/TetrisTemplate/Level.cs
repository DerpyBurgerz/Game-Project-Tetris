using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

class Level
{
	int currentLevel, linesNeededPerLevel;
	public int totalLines;
	double fallSpeed;
	public Level() 
	{
		currentLevel = 0;
		totalLines = 0;
		linesNeededPerLevel = 10;
	}
	public void CheckLevel()
	{
		if ((RecursiveSum(currentLevel + 1)) * linesNeededPerLevel <= totalLines)
			currentLevel += 1;
		//Als linesNeededPerLevel 10 is heb je 10 lines per level nodig om naar het volgende level te gaan
		//Voor level 1 heb je 10 lines nodig
		//Voor level 2, 30 lines (10+20)
		//voor level 3, 60 lines (10+20+30) etc.

		fallSpeed = Math.Pow(0.8 - (currentLevel - 1) * 0.007, currentLevel - 1);//FallSpeed wordt brekent met (0.8 - ((level - 1) * 0.007))^(level-1)
	}																			 //Dit is volgens de tetris guidelines uit 2009
	public int RecursiveSum(int num)
	{
		//Deze methode return n+(n-1)+(n-2)+(n-3)+(n-4)... tot (n-m) = 0
		if (num == 1)
			return 1;
		else
			return num + RecursiveSum(num - 1);
	}
	public void Reset()
	{
		currentLevel = 0;
		totalLines = 0;
	}
	public int CurrentLevel {  get { return currentLevel; } }
	public double FallSpeed { get { return fallSpeed; } }
}
