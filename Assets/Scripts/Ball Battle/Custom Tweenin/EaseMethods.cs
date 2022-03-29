using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaseMethods : MonoBehaviour
{
		/// <summary>This will take the input linear 0..1 value, and return a transformed version based on the specified easing function.</summary>
		public float Smooth(EaseTypes ease, float x)
		{
			switch (ease)
			{
				case EaseTypes.Smooth:
				{
					x = x * x * (3.0f - 2.0f * x);
				}
				break;

				case EaseTypes.Accelerate:
				{
					x *= x;
				}
				break;

				case EaseTypes.Decelerate:
				{
					x = 1.0f - x;
					x *= x;
					x = 1.0f - x;
				}
				break;

				// case EaseTypes.Elastic:
				// {
				// 	var angle   = x * Mathf.PI * 4.0f;
				// 	var weightA = 1.0f - Mathf.Pow(x, 0.125f);
				// 	var weightB = 1.0f - Mathf.Pow(1.0f - x, 8.0f);

				// 	x = Mathf.LerpUnclamped(0.0f, 1.0f - Mathf.Cos(angle) * weightA, weightB);
				// }
				// break;

				case EaseTypes.Elastic:
				{
					float b = 4f;
					float s = 3f;
					float alpha = s / 100f;

					float treshold = 0.005f / Mathf.Pow(10, s);
					float limit = Mathf.Floor(Mathf.Log(treshold) / -alpha);
					float omega = (b + 0.5f) * Mathf.PI/limit;

					float t = x * limit;

					// x = 1 - Mathf.Pow((float)System.Math.E, alpha*t) * Mathf.Cos(omega * t);
					// x = Mathf.LerpUnclamped(0.0f, 1.0f, x);
					// Debug.Log(Mathf.Log(0.000005f, 10f));


					var angle   = x * Mathf.PI * 1.0f * b;
					var weightA = 1.0f - Mathf.Pow(x, 0.125f / s);
					var weightB = 1.0f - Mathf.Pow(1.0f - x, 8.0f * s);

					x = Mathf.LerpUnclamped(0.0f, 1.0f - Mathf.Cos(angle) * weightA, weightB);
				}
				break;

				case EaseTypes.Back:
				{
					x = 1.0f - x;
					x = x * x * x - x * Mathf.Sin(x * Mathf.PI);
					x = 1.0f - x;
				}
				break;

				case EaseTypes.Bounce:
				{
					if (x < (4f/11f))
					{
						x = (121f/16f) * x * x;
					}
					else if (x < (8f/11f))
					{
						x = (121f/16f) * (x - (6f/11f)) * (x - (6f/11f)) + 0.75f;
					}
					else if (x < (10f/11f))
					{
						x = (121f/16f) * (x - (9f/11f)) * (x - (9f/11f)) + (15f/16f);
					}
					else
					{
						x = (121f/16f) * (x - (21f/22f)) * (x - (21f/22f)) + (63f/64f);
					}
				}
				break;

				case EaseTypes.SineIn: return 1 - Mathf.Cos((x * Mathf.PI) / 2.0f);

				case EaseTypes.SineOut: return Mathf.Sin((x * Mathf.PI) / 2.0f);

				case EaseTypes.SineInOut: return -(Mathf.Cos(Mathf.PI * x) - 1.0f) / 2.0f;

				case EaseTypes.QuadIn: return SmoothQuad(x);

				case EaseTypes.QuadOut: return 1 - SmoothQuad(1 - x);

				case EaseTypes.QuadInOut: return x < 0.5f ? SmoothQuad(x * 2) / 2 : 1 - SmoothQuad(2 - x * 2) / 2;

				case EaseTypes.CubicIn: return SmoothCubic(x);

				case EaseTypes.CubicOut: return 1 - SmoothCubic(1 - x);

				case EaseTypes.CubicInOut: return x < 0.5f ? SmoothCubic(x * 2) / 2 : 1 - SmoothCubic(2 - x * 2) / 2;

				case EaseTypes.QuartIn: return SmoothQuart(x);

				case EaseTypes.QuartOut: return 1 - SmoothQuart(1 - x);

				case EaseTypes.QuartInOut: return x < 0.5f ? SmoothQuart(x * 2) / 2 : 1 - SmoothQuart(2 - x * 2) / 2;

				case EaseTypes.QuintIn: return SmoothQuint(x);

				case EaseTypes.QuintOut: return 1 - SmoothQuint(1 - x);

				case EaseTypes.QuintInOut: return x < 0.5f ? SmoothQuint(x * 2) / 2 : 1 - SmoothQuint(2 - x * 2) / 2;

				case EaseTypes.ExpoIn: return SmoothExpo(x);

				case EaseTypes.ExpoOut: return 1 - SmoothExpo(1 - x);

				case EaseTypes.ExpoInOut: return x < 0.5f ? SmoothExpo(x * 2) / 2 : 1 - SmoothExpo(2 - x * 2) / 2;

				case EaseTypes.CircIn: return SmoothCirc(x);

				case EaseTypes.CircOut: return 1 - SmoothCirc(1 - x);

				case EaseTypes.CircInOut: return x < 0.5f ? SmoothCirc(x * 2) / 2 : 1 - SmoothCirc(2 - x * 2) / 2;

				case EaseTypes.BackIn: return SmoothBack(x);

				case EaseTypes.BackOut: return 1 - SmoothBack(1 - x);

				case EaseTypes.BackInOut: return x < 0.5f ? SmoothBack(x * 2) / 2 : 1 - SmoothBack(2 - x * 2) / 2;

				case EaseTypes.ElasticIn: return SmoothElastic(x);

				case EaseTypes.ElasticOut: return 1 - SmoothElastic(1 - x);

				case EaseTypes.ElasticInOut: return x < 0.5f ? SmoothElastic(x * 2) / 2 : 1 - SmoothElastic(2 - x * 2) / 2;

				case EaseTypes.BounceIn: return 1 - SmoothBounce(1 - x);

				case EaseTypes.BounceOut: return SmoothBounce(x);

				case EaseTypes.BounceInOut: return x < 0.5f ? 0.5f - SmoothBounce(1 - x * 2) / 2 : 0.5f + SmoothBounce(x * 2 - 1) / 2;
			}

			return x;
		}

		private static float SmoothQuad(float x)
		{
			return x * x;
		}

		private static float SmoothCubic(float x)
		{
			return x * x * x;
		}

		private static float SmoothQuart(float x)
		{
			return x * x * x * x;
		}

		private static float SmoothQuint(float x)
		{
			return x * x * x * x * x;
		}

		private static float SmoothExpo(float x)
		{
			return x == 0.0f ? 0.0f : Mathf.Pow(2.0f, 10.0f * x - 10.0f);
		}

		private static float SmoothCirc(float x)
		{
			return 1.0f - Mathf.Sqrt(1.0f - Mathf.Pow(x, 2.0f));
		}

		private static float SmoothBack(float x)
		{
			return 2.70158f * x * x * x - 1.70158f * x * x;
		}

		private static float SmoothElastic(float x)
		{
			return x == 0.0f ? 0.0f : x == 1.0f ? 1.0f : -Mathf.Pow(2.0f, 10.0f * x - 10.0f) * Mathf.Sin((x * 10.0f - 10.75f) * ((2.0f * Mathf.PI) / 3.0f));
		}

		private static float SmoothBounce(float x)
		{
			if (x < (4f/11f))
			{
				return (121f/16f) * x * x;
			}
			else if (x < (8f/11f))
			{
				return (121f/16f) * (x - (6f/11f)) * (x - (6f/11f)) + 0.75f;
			}
			else if (x < (10f/11f))
			{
				return (121f/16f) * (x - (9f/11f)) * (x - (9f/11f)) + (15f/16f);
			}
			else
			{
				return (121f/16f) * (x - (21f/22f)) * (x - (21f/22f)) + (63f/64f);
			}
		}
	}
