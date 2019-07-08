using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DigiTrailApp.Helpers
{
    /// <summary>
    /// For handling Animations
    /// </summary>
    public static class AnimatorExtensions
    {
        //ANIMATION HANDLER!   
        public static Task StartAsync(this Animator animator)
        {
            var listener = new TaskAnimationListener();
            animator.AddListener(listener);
            animator.Start();
            return listener.Task;
        }
    }
    public class TaskAnimationListener : Java.Lang.Object, Animator.IAnimatorListener
    {
        private readonly TaskCompletionSource<int> _tcs;
        public Task Task => _tcs.Task;

        public TaskAnimationListener()
        {
            _tcs = new TaskCompletionSource<int>();
        }

        public void OnAnimationCancel(Animator animation)
        {
            _tcs.TrySetCanceled();
        }

        public void OnAnimationEnd(Animator animation)
        {
            _tcs.TrySetResult(0);
        }

        public void OnAnimationRepeat(Animator animation)
        {
        }

        public void OnAnimationStart(Animator animation)
        {
        }
    }
}