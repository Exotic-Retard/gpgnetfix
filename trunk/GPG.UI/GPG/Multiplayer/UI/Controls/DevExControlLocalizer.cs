namespace GPG.Multiplayer.UI.Controls
{
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using System;

    public class DevExControlLocalizer : Localizer
    {
        public override string GetLocalizedString(StringId id)
        {
            switch (id)
            {
                case StringId.None:
                    return "";

                case StringId.CaptionError:
                    return Loc.Get("<LOC>Error");

                case StringId.InvalidValueText:
                    return Loc.Get("<LOC>Invalid Value");

                case StringId.CheckChecked:
                    return Loc.Get("<LOC>Checked");

                case StringId.CheckUnchecked:
                    return Loc.Get("<LOC>Unchecked");

                case StringId.CheckIndeterminate:
                    return Loc.Get("<LOC>Indeterminate");

                case StringId.DateEditToday:
                    return Loc.Get("<LOC>Today");

                case StringId.DateEditClear:
                    return Loc.Get("<LOC>Clear");

                case StringId.OK:
                    return Loc.Get("<LOC>OK");

                case StringId.Cancel:
                    return Loc.Get("<LOC>Cancel");

                case StringId.NavigatorFirstButtonHint:
                    return Loc.Get("<LOC>First");

                case StringId.NavigatorPreviousButtonHint:
                    return Loc.Get("<LOC>Previous");

                case StringId.NavigatorPreviousPageButtonHint:
                    return Loc.Get("<LOC>Previous Page");

                case StringId.NavigatorNextButtonHint:
                    return Loc.Get("<LOC>Next");

                case StringId.NavigatorNextPageButtonHint:
                    return Loc.Get("<LOC>Next Page");

                case StringId.NavigatorLastButtonHint:
                    return Loc.Get("<LOC>Last");

                case StringId.NavigatorAppendButtonHint:
                    return Loc.Get("<LOC>Append");

                case StringId.NavigatorRemoveButtonHint:
                    return Loc.Get("<LOC>Delete");

                case StringId.NavigatorEditButtonHint:
                    return Loc.Get("<LOC>Edit");

                case StringId.NavigatorEndEditButtonHint:
                    return Loc.Get("<LOC>End Edit");

                case StringId.NavigatorCancelEditButtonHint:
                    return Loc.Get("<LOC>Cancel Edit");

                case StringId.NavigatorTextStringFormat:
                    return Loc.Get("<LOC>Record {0} of {1}");

                case StringId.PictureEditMenuCut:
                    return Loc.Get("<LOC>Cut");

                case StringId.PictureEditMenuCopy:
                    return Loc.Get("<LOC>Copy");

                case StringId.PictureEditMenuPaste:
                    return Loc.Get("<LOC>Paste");

                case StringId.PictureEditMenuDelete:
                    return Loc.Get("<LOC>Delete");

                case StringId.PictureEditMenuLoad:
                    return Loc.Get("<LOC>Load");

                case StringId.PictureEditMenuSave:
                    return Loc.Get("<LOC>Save");

                case StringId.PictureEditOpenFileFilter:
                    return Loc.Get("<LOC>Bitmap Files (*.bmp)|*.bmp|Graphics Interchange Format (*.gif)|*.gif|JPEG File Interchange Format (*.jpg;*.jpeg)|*.jpg;*.jpeg|Icon Files (*.ico)|*.ico|All Picture Files |*.bmp;*.gif;*.jpg;*.jpeg;*.ico;*.png;*.tif|All Files |*.*");

                case StringId.PictureEditSaveFileFilter:
                    return Loc.Get("<LOC>Bitmap Files (*.bmp)|*.bmp|Graphics Interchange Format (*.gif)|*.gif|JPEG File Interchange Format (*.jpg)|*.jpg");

                case StringId.PictureEditOpenFileTitle:
                    return Loc.Get("<LOC>Open");

                case StringId.PictureEditSaveFileTitle:
                    return Loc.Get("<LOC>Save As");

                case StringId.PictureEditOpenFileError:
                    return Loc.Get("<LOC>Wrong picture format");

                case StringId.PictureEditOpenFileErrorCaption:
                    return Loc.Get("<LOC>Open error");

                case StringId.LookUpEditValueIsNull:
                    return Loc.Get("<LOC>[EditValue is null]");

                case StringId.LookUpInvalidEditValueType:
                    return Loc.Get("<LOC>Invalid LookUpEdit EditValue type.");

                case StringId.LookUpColumnDefaultName:
                    return Loc.Get("<LOC>Name");

                case StringId.MaskBoxValidateError:
                    return Loc.Get("<LOC>The entered value is incomplete.  Do you want to correct it?\r\n\r\nYes - return to the editor and correct the value.\r\nNo - leave the value as is.\r\nCancel - reset to the previous value.\r\n");

                case StringId.UnknownPictureFormat:
                    return Loc.Get("<LOC>Unknown picture format");

                case StringId.DataEmpty:
                    return Loc.Get("<LOC>No image data");

                case StringId.NotValidArrayLength:
                    return Loc.Get("<LOC>Not valid array length.");

                case StringId.ImagePopupEmpty:
                    return Loc.Get("<LOC>(Empty)");

                case StringId.ImagePopupPicture:
                    return Loc.Get("<LOC>(Picture)");

                case StringId.ColorTabCustom:
                    return Loc.Get("<LOC>Custom");

                case StringId.ColorTabWeb:
                    return Loc.Get("<LOC>Web");

                case StringId.ColorTabSystem:
                    return Loc.Get("<LOC>System");

                case StringId.CalcButtonMC:
                    return Loc.Get("<LOC>MC");

                case StringId.CalcButtonMR:
                    return Loc.Get("<LOC>MR");

                case StringId.CalcButtonMS:
                    return Loc.Get("<LOC>MS");

                case StringId.CalcButtonMx:
                    return Loc.Get("<LOC>M+");

                case StringId.CalcButtonSqrt:
                    return Loc.Get("<LOC>sqrt");

                case StringId.CalcButtonBack:
                    return Loc.Get("<LOC>Back");

                case StringId.CalcButtonCE:
                    return Loc.Get("<LOC>CE");

                case StringId.CalcButtonC:
                    return Loc.Get("<LOC>C");

                case StringId.CalcError:
                    return Loc.Get("<LOC>Calculation Error");

                case StringId.TabHeaderButtonPrev:
                    return Loc.Get("<LOC>Scroll Left");

                case StringId.TabHeaderButtonNext:
                    return Loc.Get("<LOC>Scroll Right");

                case StringId.TabHeaderButtonClose:
                    return Loc.Get("<LOC>Close");

                case StringId.XtraMessageBoxOkButtonText:
                    return Loc.Get("<LOC>OK");

                case StringId.XtraMessageBoxCancelButtonText:
                    return Loc.Get("<LOC>Cancel");

                case StringId.XtraMessageBoxYesButtonText:
                    return Loc.Get("<LOC>Yes");

                case StringId.XtraMessageBoxNoButtonText:
                    return Loc.Get("<LOC>No");

                case StringId.XtraMessageBoxAbortButtonText:
                    return Loc.Get("<LOC>Abort");

                case StringId.XtraMessageBoxRetryButtonText:
                    return Loc.Get("<LOC>Retry");

                case StringId.XtraMessageBoxIgnoreButtonText:
                    return Loc.Get("<LOC>Ignore");

                case StringId.TextEditMenuUndo:
                    return Loc.Get("<LOC>Undo");

                case StringId.TextEditMenuCut:
                    return Loc.Get("<LOC>Cut");

                case StringId.TextEditMenuCopy:
                    return Loc.Get("<LOC>Copy");

                case StringId.TextEditMenuPaste:
                    return Loc.Get("<LOC>Paste");

                case StringId.TextEditMenuDelete:
                    return Loc.Get("<LOC>Delete");

                case StringId.TextEditMenuSelectAll:
                    return Loc.Get("<LOC>Select All");

                case StringId.FilterGroupAnd:
                    return Loc.Get("<LOC>And");

                case StringId.FilterGroupNotAnd:
                    return Loc.Get("<LOC>Not And");

                case StringId.FilterGroupNotOr:
                    return Loc.Get("<LOC>Not Or");

                case StringId.FilterGroupOr:
                    return Loc.Get("<LOC>Or");

                case StringId.FilterClauseAnyOf:
                    return Loc.Get("<LOC>Is any of");

                case StringId.FilterClauseBeginsWith:
                    return Loc.Get("<LOC>Begins with");

                case StringId.FilterClauseBetween:
                    return Loc.Get("<LOC>Is between");

                case StringId.FilterClauseBetweenAnd:
                    return Loc.Get("<LOC>and");

                case StringId.FilterClauseContains:
                    return Loc.Get("<LOC>Contains");

                case StringId.FilterClauseEndsWith:
                    return Loc.Get("<LOC>Ends with");

                case StringId.FilterClauseEquals:
                    return Loc.Get("<LOC>Equals");

                case StringId.FilterClauseGreater:
                    return Loc.Get("<LOC>Is greater than");

                case StringId.FilterClauseGreaterOrEqual:
                    return Loc.Get("<LOC>Is greater than or equal to");

                case StringId.FilterClauseIsNotNull:
                    return Loc.Get("<LOC>Is not blank");

                case StringId.FilterClauseIsNull:
                    return Loc.Get("<LOC>Is blank");

                case StringId.FilterClauseLess:
                    return Loc.Get("<LOC>Is less than");

                case StringId.FilterClauseLessOrEqual:
                    return Loc.Get("<LOC>Is less than or equal to");

                case StringId.FilterClauseLike:
                    return Loc.Get("<LOC>Is like");

                case StringId.FilterClauseNoneOf:
                    return Loc.Get("<LOC>Is none of");

                case StringId.FilterClauseNotBetween:
                    return Loc.Get("<LOC>Is not between");

                case StringId.FilterClauseDoesNotContain:
                    return Loc.Get("<LOC>Does not contain");

                case StringId.FilterClauseDoesNotEqual:
                    return Loc.Get("<LOC>Does not equal");

                case StringId.FilterClauseNotLike:
                    return Loc.Get("<LOC>Is not like");

                case StringId.FilterEmptyEnter:
                    return Loc.Get("<LOC><enter a value>");

                case StringId.FilterEmptyValue:
                    return Loc.Get("<LOC><empty>");

                case StringId.FilterMenuConditionAdd:
                    return Loc.Get("<LOC>Add Condition");

                case StringId.FilterMenuGroupAdd:
                    return Loc.Get("<LOC>Add Group");

                case StringId.FilterMenuClearAll:
                    return Loc.Get("<LOC>Clear All");

                case StringId.FilterMenuRowRemove:
                    return Loc.Get("<LOC>Remove Group");

                case StringId.FilterToolTipNodeAdd:
                    return Loc.Get("<LOC>Adds a new condition to this group.");

                case StringId.FilterToolTipNodeRemove:
                    return Loc.Get("<LOC>Removes this condition.");

                case StringId.FilterToolTipNodeAction:
                    return Loc.Get("<LOC>Actions.");

                case StringId.FilterToolTipValueType:
                    return Loc.Get("<LOC>Compare to a value / another field's value.");

                case StringId.FilterToolTipElementAdd:
                    return Loc.Get("<LOC>Adds a new item to the list.");

                case StringId.FilterToolTipKeysAdd:
                    return Loc.Get("<LOC>(Use the Insert or Add key)");

                case StringId.FilterToolTipKeysRemove:
                    return Loc.Get("<LOC>(Use the Delete or Subtract key)");
            }
            return ("Localize me: " + base.GetLocalizedString(id));
        }
    }
}

