using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    public interface IToggleButton
    {
        ButtonGroup ButtonGroup { get; set; }

        void IsButtonClick(bool click);
    }

    [ObjectSystem]
    public class ButtonGroupAwakeSystem : AwakeSystem<ButtonGroup>
    {
        public override void Awake(ButtonGroup self)
        {
            self.Awake();
        }
    }

    public class ButtonGroup : Component
    {
        public IToggleButton LastButton;

        public List<IToggleButton> ButtonList;

        public void Awake()
        {
            if (ButtonList == null) ButtonList = new List<IToggleButton>();
            else ButtonList.Clear();

            LastButton = null;
        }

        public void AddButton(IToggleButton button, bool isSelected = true)
        {
            if (ButtonList.Contains(button)) return;

            ButtonList?.Add(button);
            button.ButtonGroup = this;

            if (isSelected)
            {
                if (LastButton != null)
                {
                    button.IsButtonClick(false);
                    return;
                }

                button.IsButtonClick(true);

                LastButton = button;
            }
        }

        public bool RemoveButton(IToggleButton button, IToggleButton clickButton = null)
        {
            if (LastButton == button) LastButton = null;

            if (!ButtonList.Contains(button)) return false;

            clickButton?.IsButtonClick(true);

            button.ButtonGroup = null;
            return ButtonList.Remove(button);
        }

        public void OnButtonClick(IToggleButton button)
        {
            if (LastButton != button)
                LastButton?.IsButtonClick(false);

            button.IsButtonClick(true);

            LastButton = button;
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();

            this.ButtonList?.Clear();
            this.ButtonList = null;
            LastButton = null;
        }
    }
}