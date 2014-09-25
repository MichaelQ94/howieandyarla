using UnityEngine;
using System;
using System.Timers;

public abstract class MetaS : MonoBehaviour
{	
	public HowieCtrl HowieController;
	public YarlaCtrl YarlaController;
	public ChompCtrl ChompController;
	public double Duration;
	public Timer timer;

	public MetaS ()
	{
		
	}
	
	public MetaS(HowieCtrl howieCtrl, YarlaCtrl yarlaCtrl, ChompCtrl chompCtrl)
	{
		HowieController = howieCtrl;
		YarlaController = yarlaCtrl;
		ChompController = chompCtrl;
	}
		
	public abstract bool isAllowed(int redEnergy, int blueEnergy, int purpleEnergy);

	public abstract void activate(HowieS howie, YarlaS yarla, NewChompS chomp);

	public abstract void deactivate();
}