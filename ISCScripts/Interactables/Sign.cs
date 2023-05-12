using InSheepsClothing.Events;
using VRTools.Interaction;

namespace InSheepsClothing
{
    public class Sign : GrabbableObject
    {
        public Vote signVote = Vote.None;

        public override bool Grab(Grabber grabber, bool parentGrabbedObject = false)
        {
            if(base.Grab(grabber, parentGrabbedObject))
            {
                PlayerVotedEventArgs e = new PlayerVotedEventArgs()
                {
                    vote = signVote
                };
				EventManager.Instance.OnPlayerVoted(this, e);
                return true;
			}
            return false;
        }
    }
}