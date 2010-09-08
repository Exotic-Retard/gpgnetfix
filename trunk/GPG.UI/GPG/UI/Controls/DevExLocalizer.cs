namespace GPG.UI.Controls
{
    using DevExpress.XtraGrid.Localization;
    using GPG;
    using System;

    public class DevExLocalizer : GridLocalizer
    {
        public override string GetLocalizedString(GridStringId id)
        {
            switch (id)
            {
                case GridStringId.FileIsNotFoundError:
                    return Loc.Get("<LOC>File {0} is not found");

                case GridStringId.ColumnViewExceptionMessage:
                    return Loc.Get("<LOC> Do you want to correct the value ?");

                case GridStringId.CustomizationCaption:
                    return Loc.Get("<LOC>Customization");

                case GridStringId.CustomizationColumns:
                    return Loc.Get("<LOC>Columns");

                case GridStringId.CustomizationBands:
                    return Loc.Get("<LOC>Bands");

                case GridStringId.FilterPanelCustomizeButton:
                    return Loc.Get("<LOC>Edit Filter");

                case GridStringId.PopupFilterAll:
                    return Loc.Get("<LOC>(All)");

                case GridStringId.PopupFilterCustom:
                    return Loc.Get("<LOC>(Custom)");

                case GridStringId.PopupFilterBlanks:
                    return Loc.Get("<LOC>(Blanks)");

                case GridStringId.PopupFilterNonBlanks:
                    return Loc.Get("<LOC>(Non blanks)");

                case GridStringId.CustomFilterDialogFormCaption:
                    return Loc.Get("<LOC>Custom AutoFilter");

                case GridStringId.CustomFilterDialogCaption:
                    return Loc.Get("<LOC>Show rows where:");

                case GridStringId.CustomFilterDialogRadioAnd:
                    return Loc.Get("<LOC>And");

                case GridStringId.CustomFilterDialogRadioOr:
                    return Loc.Get("<LOC>Or");

                case GridStringId.CustomFilterDialogOkButton:
                    return Loc.Get("<LOC>Ok");

                case GridStringId.CustomFilterDialogClearFilter:
                    return Loc.Get("<LOC>Clear Filter");

                case GridStringId.CustomFilterDialog2FieldCheck:
                    return Loc.Get("<LOC>Field");

                case GridStringId.CustomFilterDialogCancelButton:
                    return Loc.Get("<LOC>Cancel");

                case GridStringId.CustomFilterDialogConditionEQU:
                    return Loc.Get("<LOC>equals");

                case GridStringId.CustomFilterDialogConditionNEQ:
                    return Loc.Get("<LOC>does not equal");

                case GridStringId.CustomFilterDialogConditionGT:
                    return Loc.Get("<LOC>is greater than");

                case GridStringId.CustomFilterDialogConditionGTE:
                    return Loc.Get("<LOC>is greater than or equal to");

                case GridStringId.CustomFilterDialogConditionLT:
                    return Loc.Get("<LOC>is less than");

                case GridStringId.CustomFilterDialogConditionLTE:
                    return Loc.Get("<LOC>is less than or equal to");

                case GridStringId.CustomFilterDialogConditionBlanks:
                    return Loc.Get("<LOC>blanks");

                case GridStringId.CustomFilterDialogConditionNonBlanks:
                    return Loc.Get("<LOC>non blanks");

                case GridStringId.CustomFilterDialogConditionLike:
                    return Loc.Get("<LOC>like");

                case GridStringId.CustomFilterDialogConditionNotLike:
                    return Loc.Get("<LOC>not like");

                case GridStringId.WindowErrorCaption:
                    return Loc.Get("<LOC>");

                case GridStringId.MenuFooterSum:
                    return Loc.Get("<LOC>Sum");

                case GridStringId.MenuFooterMin:
                    return Loc.Get("<LOC>Min");

                case GridStringId.MenuFooterMax:
                    return Loc.Get("<LOC>Max");

                case GridStringId.MenuFooterCount:
                    return Loc.Get("<LOC>Count");

                case GridStringId.MenuFooterAverage:
                    return Loc.Get("<LOC>Average");

                case GridStringId.MenuFooterNone:
                    return Loc.Get("<LOC>None");

                case GridStringId.MenuFooterSumFormat:
                    return Loc.Get("<LOC>SUM={0:#.##}");

                case GridStringId.MenuFooterMinFormat:
                    return Loc.Get("<LOC>MIN={0}");

                case GridStringId.MenuFooterMaxFormat:
                    return Loc.Get("<LOC>MAX={0}");

                case GridStringId.MenuFooterCountFormat:
                    return Loc.Get("<LOC>{0}");

                case GridStringId.MenuFooterAverageFormat:
                    return Loc.Get("<LOC>AVR={0:#.##}");

                case GridStringId.MenuColumnSortAscending:
                    return Loc.Get("<LOC>Sort Ascending");

                case GridStringId.MenuColumnSortDescending:
                    return Loc.Get("<LOC>Sort Descending");

                case GridStringId.MenuColumnGroup:
                    return Loc.Get("<LOC>Group By This Column");

                case GridStringId.MenuColumnUnGroup:
                    return Loc.Get("<LOC>UnGroup");

                case GridStringId.MenuColumnColumnCustomization:
                    return Loc.Get("<LOC>Column Chooser");

                case GridStringId.MenuColumnBestFit:
                    return Loc.Get("<LOC>Best Fit");

                case GridStringId.MenuColumnFilter:
                    return Loc.Get("<LOC>Can Filter");

                case GridStringId.MenuColumnClearFilter:
                    return Loc.Get("<LOC>Clear Filter");

                case GridStringId.MenuColumnBestFitAllColumns:
                    return Loc.Get("<LOC>Best Fit (all columns)");

                case GridStringId.MenuGroupPanelFullExpand:
                    return Loc.Get("<LOC>Full Expand");

                case GridStringId.MenuGroupPanelFullCollapse:
                    return Loc.Get("<LOC>Full Collapse");

                case GridStringId.MenuGroupPanelClearGrouping:
                    return Loc.Get("<LOC>Clear Grouping");

                case GridStringId.PrintDesignerGridView:
                    return Loc.Get("<LOC>Print Settings (Grid View)");

                case GridStringId.PrintDesignerCardView:
                    return Loc.Get("<LOC>Print Settings (Card View)");

                case GridStringId.PrintDesignerBandedView:
                    return Loc.Get("<LOC>Print Settings (Banded View)");

                case GridStringId.PrintDesignerBandHeader:
                    return Loc.Get("<LOC>BandHeader");

                case GridStringId.MenuColumnGroupBox:
                    return Loc.Get("<LOC>Group By Box");

                case GridStringId.CardViewNewCard:
                    return Loc.Get("<LOC>New Card");

                case GridStringId.CardViewQuickCustomizationButton:
                    return Loc.Get("<LOC>Customize");

                case GridStringId.CardViewQuickCustomizationButtonFilter:
                    return Loc.Get("<LOC>Filter");

                case GridStringId.CardViewQuickCustomizationButtonSort:
                    return Loc.Get("<LOC>Sort:");

                case GridStringId.GridGroupPanelText:
                    return Loc.Get("<LOC>Drag a column header here to group by that column");

                case GridStringId.GridNewRowText:
                    return Loc.Get("<LOC>Click here to add a new row");

                case GridStringId.GridOutlookIntervals:
                    return Loc.Get("<LOC>Older;Last Month;Earlier this Month;Three Weeks Ago;Two Weeks Ago;Last Week;;;;;;;;Yesterday;Today;Tomorrow;;;;;;;;Next Week;Two Weeks Away;Three Weeks Away;Later this Month;Next Month;Beyond Next Month;");

                case GridStringId.PrintDesignerDescription:
                    return Loc.Get("<LOC>Set up various printing options for the current view.");

                case GridStringId.MenuFooterCustomFormat:
                    return Loc.Get("<LOC>Custom={0}");

                case GridStringId.MenuFooterCountGroupFormat:
                    return Loc.Get("<LOC>Count={0}");

                case GridStringId.MenuColumnClearSorting:
                    return Loc.Get("<LOC>Clear Sorting");

                case GridStringId.MenuColumnFilterEditor:
                    return Loc.Get("<LOC>Filter Editor");
            }
            return base.GetLocalizedString(id);
        }
    }
}

