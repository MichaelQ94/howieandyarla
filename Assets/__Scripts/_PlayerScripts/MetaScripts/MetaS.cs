using UnityEngine;
using System;
using System.Timers;

public abstract class MetaS : MonoBehaviour
{	
	public float DURATION_MAX;

	public HowieCtrl HowieController;
	public YarlaCtrl YarlaController;
	public ChompCtrl ChompController;
	public float duration;
	public Timer timer;

	public MetaS ()
	{
		
	}

	//Every metamorphosis should call this super constructor to pass its set of controllers
	public MetaS(HowieCtrl howieCtrl, YarlaCtrl yarlaCtrl, ChompCtrl chompCtrl)
	{
		HowieController = howieCtrl;
		YarlaController = yarlaCtrl;
		ChompController = chompCtrl;
	}


	//ABSTRACT METHODS

	//Every metamorphosis should be able to return its cost for each energy type
	public abstract int getBlueCost(); 
	public abstract int getRedCost(); 
	public abstract int getPurpleCost(); 

	//Every metamorphosis should be able to check if it is available based on Howie's energy values
	public abstract bool isAllowed(int redEnergy, int blueEnergy, int purpleEnergy);

	//Every metamorphosis should have this method to properly set all attributes, load controllers, etc.
	public abstract void activate(HowieS howie, YarlaS yarla, NewChompS chomp);

	//Every metamorphosis should be able to "clean up after itself" after it ends
	public abstract void deactivate();
}