using Avalonia;
using Avalonia.Controls;

namespace BoilerplateAvaloniaApp.WebViewImplementation;

/// <summary>
/// Aggregator Window code section defining the TabDragInfo helper class used in the drag drop of tab items
/// </summary>
public partial class AggregatorWindow {
    /// <summary>
    /// TabDragInfo helper class
    /// </summary>
    /// <remarks>
    /// Represents a linked list of tab items
    /// </remarks>
    private class TabDragInfo {
        public readonly TabItem TabItem;
        public readonly int Width;
        public readonly int HalfWidth;

        public int Index;
        public PixelPoint OriginPoint;

        private TabDragInfo tabBefore;
        private TabDragInfo tabAfter;

        public TabDragInfo(TabItem tabItemArg, int indexArg, PixelPoint originPointArg, int widthArg, TabDragInfo tabBeforeArg) {
            TabItem = tabItemArg;
            Index = indexArg;
            OriginPoint = originPointArg;
            Width = widthArg;
            HalfWidth = Width / 2;
            tabBefore = tabBeforeArg;
            if (!(tabBefore is null)) {
                tabBefore.tabAfter = this;
            }
            tabAfter = null;
        }

        public int GetMinWidth() {
            // Move to first tab drag info
            var tab = this;
            while (!(tab.tabBefore is null)) {
                tab = tab.tabBefore;
            }

            var minWidth = int.MaxValue;
            do {
                minWidth = Math.Min(minWidth, tab.Width);
                tab = tab.tabAfter;
            } while (!(tab is null));
            return minWidth;
        }

        public bool AdjustPosition(int xOriginAdjusted, int tabOffset, out int originalIndex, out int moveToIndex) {
            // Determine if the tab being dragged has moved sufficiently close enough to the previous or next tab
            var isAdjusted = false;
            originalIndex = Index;
            if (!(tabBefore is null) && (xOriginAdjusted < (tabBefore.OriginPoint.X + tabOffset))) {
                // Switch this tab with the previous tab

                // Move this tab's origin point to the tabBefore's origin point
                OriginPoint = tabBefore.OriginPoint;
                // Move the tabBefore's origin point by this tab's width
                tabBefore.OriginPoint += new PixelPoint(Width, 0);

                // Swap indices
                Index = tabBefore.Index; // --index;
                tabBefore.Index = originalIndex; // ++tabBefore.index;

                // Swap tab links
                SwapTabs(tabBefore, this);

                isAdjusted = true;
            } else if (!(tabAfter is null) && ((xOriginAdjusted + Width) > ((tabAfter.OriginPoint.X + tabAfter.Width) - tabOffset))) {
                // Switch this tab with the next tab

                // Move the tab after's origin point to this tab's origin point
                tabAfter.OriginPoint = OriginPoint;
                // Move this tab's origin by the tabAfter's width
                OriginPoint += new PixelPoint(tabAfter.Width, 0);

                // Swap indices
                Index = tabAfter.Index; // ++index;
                tabAfter.Index = originalIndex; // --tabAfter.index;

                // Swap tab links
                SwapTabs(this, tabAfter);

                isAdjusted = true;
            }
            moveToIndex = Index;

            return isAdjusted;
        }

        private static void SwapTabs(TabDragInfo firstTab, TabDragInfo secondTab) {
            // firstTab's tabAfter gets secondTab's tabAfter
            firstTab.tabAfter = secondTab.tabAfter;
            // secondTab's tabBefore gets firstTab's tabBefore
            secondTab.tabBefore = firstTab.tabBefore;
            // firstTab's tabBefore is now secondTab
            firstTab.tabBefore = secondTab;
            // secondTab's tabAfter is now firstTab
            secondTab.tabAfter = firstTab;
        }
    }
}
