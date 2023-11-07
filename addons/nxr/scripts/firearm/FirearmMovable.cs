using Godot;
using NXRInteractable;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NXRFirearm;

[Tool]
public partial class FirearmMovable : Interactable
{
	[ExportGroup("Tool Settings")]
	[Export]
	public Node3D Target;

	[Export]
	private bool _setStartXform = false;
	[Export]
	private bool _setEndXform = false;

	[Export]
	public bool _goStart = false;
	[Export]
	public bool _goEnd = false;

	[ExportGroup("Transforms")]
	[Export]
	public Transform3D StartXform;
	[Export]
	public Transform3D EndXform;
	private bool _snapped = false;


	public void RunTool()
	{

		if (Engine.IsEditorHint())
		{
			if (Target == null)
			{
				Target = this;
			}

			if (Target != this)
			{

				if (GlobalPosition != Target.GlobalPosition && !_snapped)
				{
					GlobalPosition = Target.GlobalPosition;
					_snapped = true;
				}
			}

			if (_setStartXform)
			{
				StartXform = Transform.Orthonormalized();
				_setStartXform = false;
			}
			if (_setEndXform)
			{
				EndXform = Transform.Orthonormalized();
				_setEndXform = false;
			}

			if (_goStart)
			{
				Transform = StartXform.Orthonormalized();
				_goStart = false;
			}

			if (_goEnd)
			{
				Transform = EndXform.Orthonormalized();
				_goEnd = false;
			}
		}

		if(Target != this) {
			Target.GlobalPosition = GlobalPosition;
			Target.RotationDegrees = RotationDegrees;
		}
	
	}


	public void GoStart()
	{
		Transform = StartXform;
	}

	public void GoEnd()
	{
		Transform = EndXform;
	}

	public Vector3 GetMinOrigin()
	{
		float x = StartXform.Origin.X < EndXform.Origin.X ? StartXform.Origin.X : EndXform.Origin.X;
		float y = StartXform.Origin.Y < EndXform.Origin.Y ? StartXform.Origin.Y : EndXform.Origin.Y;
		float z = StartXform.Origin.Z < EndXform.Origin.Z ? StartXform.Origin.Z : EndXform.Origin.Z;

		return new Vector3(x, y, z);
	}

	public Vector3 GetMaxOrigin()
	{
		float x = StartXform.Origin.X > EndXform.Origin.X ? StartXform.Origin.X : EndXform.Origin.X;
		float y = StartXform.Origin.Y > EndXform.Origin.Y ? StartXform.Origin.Y : EndXform.Origin.Y;
		float z = StartXform.Origin.Z > EndXform.Origin.Z ? StartXform.Origin.Z : EndXform.Origin.Z;

		return new Vector3(x, y, z);
	}

	public Vector3 GetMinRotation()
	{
		Vector3 startEuler = StartXform.Basis.GetEuler();
		Vector3 endEuler = EndXform.Basis.GetEuler();

		float x = startEuler.X < endEuler.X ? startEuler.X : endEuler.X;
		float y = startEuler.Y < endEuler.X ? startEuler.Y : endEuler.Y;
		float z = startEuler.Z < endEuler.Z ? startEuler.Z : endEuler.Z;
		return new Vector3(x, y, z);
	}

	public Vector3 GetMaxRotation()
	{
		Vector3 startEuler = StartXform.Basis.GetEuler();
		Vector3 endEuler = EndXform.Basis.GetEuler();

		float x = startEuler.X > endEuler.X ? startEuler.X : endEuler.X;
		float y = startEuler.Y > endEuler.X ? startEuler.Y : endEuler.Y;
		float z = startEuler.Z > endEuler.Z ? startEuler.Z : endEuler.Z;
		return new Vector3(x, y, z);
	}

	public bool AtStart()
	{
		return StartXform.Orthonormalized().IsEqualApprox(Transform.Orthonormalized());
	}

	public bool AtEnd()
	{
		return EndXform.Orthonormalized().IsEqualApprox(Transform.Orthonormalized());
	}

	public void GoToStart() { 
		Transform = StartXform; 
	}	

	public void GoToEnd() { 
		Transform = EndXform; 
	}
	public void StartToEnd(float t)
	{
		t = Mathf.Clamp(t, 0, 1);
		Transform = StartXform.InterpolateWith(EndXform, t);
	}

	public void EndToStart(float t)
	{
		t = Mathf.Clamp(t, 0, 1);
		Transform = EndXform.InterpolateWith(StartXform, t);
	}

	public float MiddlePositionRatio(Vector3 pos, bool invert = false)
	{
		Vector3 start = StartXform.Origin;
		Vector3 end = EndXform.Origin;
		Vector3 dir = start - pos;



		float ratio = (start - end).Normalized().Dot(dir.Normalized());
		return ratio;
	}
}
