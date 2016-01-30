

Fingers, by Jeff Johnson
Fingers (c) 2015 Digital Ruby, LLC

Version 1.0.0

Changelog
--------------------
1.1		- Fingers can use all mouse buttons and the mouse wheel.
		- Use ctrl + wheel to pinch and shift + wheel to rotate.
		- FingersScript has Touches property and the demo adds circles for touch points.
1.0.2	- Bug fixes
		- Breaking change: OnUpdated is now just Updated

1.0.0	- Initial release

Fingers is an advanced gesture recognizer system for Unity and any other platform where C# is supported (such as Xamarin). I am using this same code in a native drawing app for Android (You Doodle) and they work great.

If you've used UIGestureRecognizer on iOS, you should feel right at home using Fingers.

Instructions
--------------------
To get started, add FingersScript.cs to your main camera. This script has two properties:
- Treat mouse as pointer (default is true, useful for testing in the player for some gestures). Disable this if you are using Unity Remote.
- Pass through views. By default all Text objects are ignored by Fingers. These are game objects that are part of the event system that will be ignored. For example, you could put a panel in this list and it would be ignored.

Once you've added the script, you will need to add some gestures. This will require you to create a C# script of your own and add a reference to the FingersScript object that you added to the main camera. See the demo script for what this looks like. Remember to add the namespace DigitalRubyShared.

Please review the Start method of DemoScript.cs to see how gestures are created and added to the finger script.

*Note* I don't use anonymous / inline delegates in the demo script as these seem to crash on iOS.

-------------------

I've created a demo scene that demonstrates most of the capabilities of Fingers, so please check it out.

At the bottom of this Readme file is the code for an Android touch adapter using Xamarin.

I'm available to answer your questions or feedback at jjxtra@gmail.com

Thank you.

- Jeff Johnson

--------------------
Xamarin Android Touch Adapter Code:

// Created by Jeff Johnson
// (c) 2015 Digital Ruby, LLC
// Source code may not be sold or re-distributed

/*
Example usage in an Android view:
public override bool DispatchTouchEvent(MotionEvent e)
{
    base.DispatchTouchEvent(e);

    this.ProcessMotionEvent(TapGesture, e);
    this.ProcessMotionEvent(PanGesture, e);
    this.ProcessMotionEvent(LongPressGesture, e);

    return true;
}
*/

using System;
using System.Collections.Generic;

using Android.Graphics;
using Android.Views;
using Android.Widget;

using DigitalRubyShared;

namespace com.digitalruby.youdoodle
{
    public static class GestureRecognizerExtension
    {
        private static readonly List<GestureTouch> touches = new List<GestureTouch>();
        private static readonly MotionEvent.PointerCoords coords = new MotionEvent.PointerCoords();
        private static readonly MotionEvent.PointerCoords coords2 = new MotionEvent.PointerCoords();
        private static readonly int[] locationOnScreen = new int[2];

        public static ICollection<GestureTouch> GetTouches(View v, MotionEvent e)
        {
            touches.Clear();
            for (int i = 0; i < e.PointerCount; i++)
            {
                int pointerId = e.GetPointerId(i);
                int pointerIndex = e.FindPointerIndex(pointerId);
                e.GetPointerCoords(pointerIndex, coords);

                if (e.HistorySize == 0 || e.HistorySize <= pointerIndex || e.GetHistoricalSize(pointerIndex) == 0)
                {
                    coords2.X = coords.X;
                    coords2.Y = coords.Y;
                }
                else
                {
                    e.GetHistoricalPointerCoords(pointerIndex, 0, coords2);
                }

                v.GetLocationOnScreen(locationOnScreen);
                GestureTouch t = new GestureTouch(pointerId, coords.X, coords.Y, coords2.X, coords2.Y, 0.0f, coords.X + locationOnScreen[0], coords.Y + locationOnScreen[1]);

                touches.Add(t);
            }

            return touches;
        }

        public static void ProcessMotionEvent(this View v, GestureRecognizer g, MotionEvent e)
        {
            v.ProcessMotionEvent(e, false, g);
        }

        public static void ProcessMotionEvent(this View v, MotionEvent e, params GestureRecognizer[] gestures)
        {
            v.ProcessMotionEvent(e, false, gestures);
        }

        public static void ProcessMotionEvent(this View v, MotionEvent e, bool inScrollView, params GestureRecognizer[] gestures)
        {
            v.ProcessMotionEvent(e, (inScrollView ? new Rect(0, 0, v.Width, v.Height) : null), gestures);
        }

        public static void ProcessMotionEvent(this View v, MotionEvent e, Rect scrollViewRect, params GestureRecognizer[] gestures)
        {
            bool gestureStarted = (e.Action == MotionEventActions.Down && scrollViewRect != null && scrollViewRect.Contains((int)e.GetX(), (int)e.GetY()));
            bool gestureEnded = (e.Action == MotionEventActions.Up);
            bool foundGesture = false;

            foreach (GestureRecognizer g in gestures)
            {
                if (e.PointerCount == 0 || (g.PlatformSpecificView != null && v != g.PlatformSpecificView))
                {
                    continue;
                }
                foundGesture = true;

                try
                {
                    switch (e.ActionMasked)
                    {
                        case MotionEventActions.Down:
                        case MotionEventActions.PointerDown:
                            g.ProcessTouchesBegan(GetTouches(v, e));
                            break;

                        case MotionEventActions.Move:
                            g.ProcessTouchesMoved(GetTouches(v, e));
                            break;

                        case MotionEventActions.Up:
                        case MotionEventActions.PointerUp:
                            g.ProcessTouchesEnded(GetTouches(v, e));
                            break;

                        case MotionEventActions.Cancel:
                            g.ProcessTouchesCancelled(GetTouches(v, e));
                            break;

                        default:
                            Logger.WriteLine("Unknown action: {0}", e.ActionMasked);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine("Error processing touches: {0}", ex);
                }
            }

            if (foundGesture && scrollViewRect != null && !scrollViewRect.IsEmpty)
            {
                ScrollView s = null;
                for (IViewParent p = v.Parent; p != null; p = p.Parent)
                {
                    s = p as ScrollView;
                    if (s != null)
                    {
                        if (gestureStarted)
                        {
                            s.RequestDisallowInterceptTouchEvent(true);
                        }
                        else if (gestureEnded)
                        {
                            s.RequestDisallowInterceptTouchEvent(false);
                        }
                    }
                }
            }
        }
    }
}