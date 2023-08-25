using Jint.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Utils;

namespace Turmerik.WinForms.Components
{
    public class MultiStateControlStyle<TControl, TState>
        where TControl : Control
    {
        private readonly IEqualityComparer<TState> stateEqCompr;
        private readonly Dictionary<TState, Func<TControl, TState, ControlStyle.Immtbl>> controlStylesMap;
        private readonly Func<TControl, TState, ControlStyle.Immtbl> defaultStyleFactory;
        private readonly ControlStyle.Immtbl initialStyle;

        private TState state;
        private ControlStyle.Immtbl style;

        public MultiStateControlStyle(
            TControl control,
            Dictionary<TState, Func<TControl, TState, ControlStyle.Immtbl>> controlStylesMap,
            Func<TControl, TState, ControlStyle.Immtbl> defaultStyleFactory = null,
            TState initialState = default,
            bool initStyle = false,
            IEqualityComparer<TState> stateEqCompr = null)
        {
            this.stateEqCompr = stateEqCompr ?? EqualityComparer<TState>.Default;
            this.Control = control ?? throw new ArgumentNullException(nameof(control));
            this.controlStylesMap = controlStylesMap ?? throw new ArgumentNullException(nameof(controlStylesMap));

            this.defaultStyleFactory = defaultStyleFactory.FirstNotNull(
                (ctrl, state) => initialStyle);

            this.initialStyle = ControlStyle.Mtbl.FromControl(control).ToImmtbl();
            this.style = initialStyle;
            this.state = initialState;
            
            SetState(initialState, initStyle);
        }

        public TState State => this.state;
        public ControlStyle.Immtbl Style => this.style;
        protected TControl Control { get; }

        public TState GetState() => state;

        public void SetState(
            TState newState,
            bool? updateStyle = null)
        {
            bool updateStyleValue = updateStyle ?? !stateEqCompr.Equals(state, newState);
            this.state = newState;

            if (updateStyleValue)
            {
                TryUpdateStyle(
                    Control,
                    newState);
            }
        }

        private void TryUpdateStyle(
            TControl control,
            TState newState)
        {
            Func<TControl, TState, ControlStyle.Immtbl> styleFactory;

            if (!controlStylesMap.TryGetValue(newState, out styleFactory))
            {
                styleFactory = defaultStyleFactory;
            }

            var newStyle = styleFactory(
                control,
                newState);

            ApplyStyle(
                control,
                newStyle);
        }

        private void ApplyStyle(
            TControl control,
            ControlStyle.IClnbl style)
        {
            control.BackColor = style.BackColor;
            control.ForeColor = style.ForeColor;

            control.Font = new Font(
                style.Font,
                style.FontStyle);
        }
    }
}
