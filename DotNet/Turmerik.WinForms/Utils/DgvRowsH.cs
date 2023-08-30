using Jint.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Turmerik.Utils;
using Turmerik.WinForms.Components;

namespace Turmerik.WinForms.Utils
{
    public static class DgvRowsH
    {
        public static DataGridViewRow Row(
            this DataGridViewCell[] cellsArr)
        {
            var row = new DataGridViewRow();
            row.Cells.AddRange(cellsArr);

            return row;
        }

        public static TDgvCell MapPropIfReq<TDgvCell, TPropVal>(
            TDgvCell cell,
            TPropVal propVal,
            Action<TDgvCell, TPropVal> propAssignFunc,
            Func<TPropVal, bool> isValidPredicate = null)
            where TDgvCell : DataGridViewCell
        {
            isValidPredicate = isValidPredicate.FirstNotNull(
                value => value != null);

            if (isValidPredicate(propVal))
            {
                propAssignFunc(cell, propVal);
            }

            return cell;
        }

        public static TDgvCell ApplyCellStyleIfNotNull<TDgvCell>(
            this TDgvCell cell,
            DataGridViewCellStyle style)
            where TDgvCell : DataGridViewCell => MapPropIfReq(
                cell, style, (c, s) => c.Style = style);

        public static TDgvCell ApplyValueIfReq<TDgvCell, TPropVal>(
            this TDgvCell cell,
            TPropVal value,
            Func<TPropVal, bool> isValidPredicate = null)
            where TDgvCell : DataGridViewCell => MapPropIfReq(
                cell,
                value,
                (c, s) => c.Value = value,
                isValidPredicate);

        public static TDgvCell Cell<TDgvCell, TCellValue>(
            DgvCellOptsCore.IClnbl<TDgvCell, TCellValue> opts)
            where TDgvCell : DataGridViewCell
        {
            var factory = opts.CellFactory.FirstNotNull(
                () => Activator.CreateInstance<TDgvCell>());

            var cell = factory();
            cell.ApplyCellStyleIfNotNull(opts.Style);

            ApplyValueIfReq(
                cell,
                opts.CellValue,
                opts.IsValidValuePredicate);

            opts.Callback?.Invoke(cell);
            return cell;
        }

        public static DataGridViewTextBoxCell TextBoxCell(
            DgvTextBoxCellOpts.IClnbl opts) => Cell(opts);

        public static DataGridViewCheckBoxCell CheckBoxCell(
            DgvCheckBoxCellOpts.IClnbl opts) => Cell(opts);

        public static DataGridViewButtonCell ButtonCell(
            DgvButtonCellOpts.IClnbl opts) => Cell(opts);

        public static DataGridViewComboBoxCell ComboBoxCell<TPropVal>(
            DgvComboBoxCellOpts.IClnbl<TPropVal> opts)
        {
            var optsMtbl = opts.AsMtbl();

            optsMtbl.Callback = cell =>
            {
                cell.DisplayMember = opts.DisplayMember;
                cell.ValueMember = opts.ValueMember;
                cell.Items.AddRange(opts.ComboBoxItems);

                ApplyValueIfReq(
                    cell,
                    opts.CellValue,
                    opts.IsValidValuePredicate);

                opts.Callback?.Invoke(cell);
            };

            var dgvCell = Cell(optsMtbl);
            return dgvCell;
        }
    }
}
