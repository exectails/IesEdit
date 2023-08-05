using System.Windows.Forms;

namespace IesEdit
{
	/// <summary>
	/// ToolStrip renderer without border.
	/// </summary>
	public class ToolStripRendererNL : ToolStripSystemRenderer
	{
		/// <summary>
		/// Skips border rendering.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			//base.OnRenderToolStripBorder(e);
		}
	}
}
