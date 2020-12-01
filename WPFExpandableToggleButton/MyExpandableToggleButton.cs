using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;

namespace WPFExpandableToggleButton
{
    public class MyExpandableToggleButton : ToggleButton
    {
        private MyExpandableToggleButtonAutomationPeer peer;

        // This example sample gives the button an initial state of collapsed.
        private ExpandCollapseState state = ExpandCollapseState.Collapsed;

        // Add a property for the current expanded/collapse state of the button.
        public ExpandCollapseState State
        {
            get
            {
                return this.state;
            }
            set
            {
                ExpandCollapseState previousState = this.state;

                this.state = value;

                // Raise a UIA property changed event if the expanded state of button has changed.
                if ((this.peer != null) && (this.state != previousState))
                {
                    this.peer.RaisePropertyChangedEvent(
                        ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty,
                        previousState,
                        this.state);
                }

                // Important: Take action here to updated the button's visuals to match the new state.
            }
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            if (this.peer == null)
            {
                this.peer = new MyExpandableToggleButtonAutomationPeer(this);
            }

            return this.peer;
        }

        protected override void OnClick()
        {
            // In response to the button being tapped or clicked, or invoked 
            // with the keyboard, change the expanded state of the button.
            this.State = (this.State == ExpandCollapseState.Collapsed ?
                ExpandCollapseState.Expanded :
                ExpandCollapseState.Collapsed);
        }
    }

    // Add support for the UIA ExpandCollapse pattern to a standard WPF ToggleButton control.
    public class MyExpandableToggleButtonAutomationPeer :
        ToggleButtonAutomationPeer,
        IExpandCollapseProvider
    {
        private MyExpandableToggleButton button;

        public MyExpandableToggleButtonAutomationPeer(MyExpandableToggleButton owner) :
            base(owner)
        {
            this.button = owner;
        }

        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.ExpandCollapse)
            {
                // Say this control does support the UIA ExpandCollapse pattern.
                return this;
            }
            else if (patternInterface == PatternInterface.Toggle)
            {
                // Say this control does not support the UIA Toggle pattern.
                return null;
            }

            return base.GetPattern(patternInterface);
        }

        public ExpandCollapseState ExpandCollapseState
        {
            get
            {
                return this.button.State;
            }
        }

        public void Expand()
        {
            this.button.State = ExpandCollapseState.Expanded;
        }

        public void Collapse()
        {
            this.button.State = ExpandCollapseState.Collapsed;
        }
    }
}
