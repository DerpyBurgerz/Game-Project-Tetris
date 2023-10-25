using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
internal class SuperRotatingSystem
{
	public int previousRotation, newRotation;
	List<Vector2> testList;
	public List<Vector2> TestList { get { return testList; } }
	public SuperRotatingSystem(int oldRotation, int newRotation, List<Vector2> testList)
	{
		this.previousRotation = oldRotation;
		this.newRotation = newRotation;
		this.testList = testList;
	}
	public SuperRotatingSystem(int num, bool I)
	{
		if (I)
		{
			if (num == 0)
				newTests(0, 1, new Vector2(0, 0), new Vector2(-2, 0), new Vector2(1, 0), new Vector2(-2, -1), new Vector2(1, 2));
			if (num == 1)
				newTests(1, 2, new Vector2(0, 0), new Vector2(-1, 0), new Vector2(2, 0), new Vector2(-1, 2), new Vector2(2, -1));
			if (num == 2)
				newTests(2, 3, new Vector2(0, 0), new Vector2(2, 0), new Vector2(-1, 0), new Vector2(2, 1), new Vector2(-1, -2));
			if (num == 3)
				newTests(3, 0, new Vector2(0, 0), new Vector2(1, 0), new Vector2(-2, 0), new Vector2(1, -2), new Vector2(-2, 1));

			if (num == 4)//DEZE NOG DOEN
				newTests(1, 0, new Vector2(0, 0), new Vector2(2, 0), new Vector2(-1, 0), new Vector2(2, 1), new Vector2(-1, -2));
			if (num == 5)
				newTests(2, 1, new Vector2(0, 0), new Vector2(1, 0), new Vector2(-2, 0), new Vector2(1, -2), new Vector2(-2, 1));
			if (num == 6)
				newTests(3, 2, new Vector2(0, 0), new Vector2(-2, 0), new Vector2(1, 0), new Vector2(-2, -1), new Vector2(1, 2));
			if (num == 7)																					
				newTests(0, 3, new Vector2(0, 0), new Vector2(-1, 0), new Vector2(2, 0), new Vector2(-1, 2), new Vector2(2, -1));
		}
		else 
		{
			if (num == 0)
				newTests(0, 1, new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, -2), new Vector2(-1, -2));
			if (num == 1) 
				newTests(1, 2, new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, 2), new Vector2(1, 2));
			if (num == 2)
				newTests(2, 3, new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, -2), new Vector2(1, -2));
			if (num == 3)
				newTests(3, 0, new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, 2), new Vector2(-1, 2));
			
			if (num == 4)
				newTests(1, 0, new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, 2), new Vector2(1, 2));
			if (num == 5)
				newTests(2, 1, new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, -2), new Vector2(-1, -2));
			if (num == 6)
				newTests(3, 2, new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, 2), new Vector2(-1, -2));
			if (num == 7)
				newTests(0, 3, new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, -2), new Vector2(1, -2));
		}
	}
	public void newTests(int previousRotation, int newRotation, Vector2 test1, Vector2 test2, Vector2 test3, Vector2 test4, Vector2 test5)
	{
		this.previousRotation = previousRotation;
		this.newRotation = newRotation;
		testList = new List<Vector2>();
		testList.Add(test1);
		testList.Add(test2);
		testList.Add(test3); 
		testList.Add(test4);
		testList.Add(test5);
	}
}
