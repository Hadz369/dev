
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.VPaned vpaned1;
	
	private global::Gtk.Fixed fixed1;
	
	private global::Gtk.Button btnStart;
	
	private global::Gtk.Button button1;
	
	private global::Gtk.Button button3;
	
	private global::Gtk.Entry textBox1;
	
	private global::Gtk.Entry textBox2;
	
	private global::Gtk.Entry tbVoltage;
	
	private global::Gtk.Entry textBox3;
	
	private global::Gtk.Notebook notebook1;
	
	private global::Gtk.Label label1;
	
	private global::Gtk.Label label2;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vpaned1 = new global::Gtk.VPaned ();
		this.vpaned1.CanFocus = true;
		this.vpaned1.Name = "vpaned1";
		// Container child vpaned1.Gtk.Paned+PanedChild
		this.fixed1 = new global::Gtk.Fixed ();
		this.fixed1.HasWindow = false;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.btnStart = new global::Gtk.Button ();
		this.btnStart.CanFocus = true;
		this.btnStart.Name = "btnStart";
		this.btnStart.UseUnderline = true;
		this.btnStart.Label = global::Mono.Unix.Catalog.GetString ("Start");
		this.fixed1.Add (this.btnStart);
		global::Gtk.Fixed.FixedChild w1 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.btnStart]));
		w1.X = 30;
		w1.Y = 21;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.button1 = new global::Gtk.Button ();
		this.button1.CanFocus = true;
		this.button1.Name = "button1";
		this.button1.UseUnderline = true;
		this.button1.Label = global::Mono.Unix.Catalog.GetString ("Close");
		this.fixed1.Add (this.button1);
		global::Gtk.Fixed.FixedChild w2 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.button1]));
		w2.X = 136;
		w2.Y = 15;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.button3 = new global::Gtk.Button ();
		this.button3.CanFocus = true;
		this.button3.Name = "button3";
		this.button3.UseUnderline = true;
		this.button3.Label = global::Mono.Unix.Catalog.GetString ("GtkButton");
		this.fixed1.Add (this.button3);
		global::Gtk.Fixed.FixedChild w3 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.button3]));
		w3.X = 226;
		w3.Y = 17;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.textBox1 = new global::Gtk.Entry ();
		this.textBox1.CanFocus = true;
		this.textBox1.Name = "textBox1";
		this.textBox1.IsEditable = true;
		this.textBox1.InvisibleChar = '●';
		this.fixed1.Add (this.textBox1);
		global::Gtk.Fixed.FixedChild w4 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.textBox1]));
		w4.X = 294;
		w4.Y = 75;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.textBox2 = new global::Gtk.Entry ();
		this.textBox2.CanFocus = true;
		this.textBox2.Name = "textBox2";
		this.textBox2.IsEditable = true;
		this.textBox2.InvisibleChar = '●';
		this.fixed1.Add (this.textBox2);
		global::Gtk.Fixed.FixedChild w5 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.textBox2]));
		w5.X = 295;
		w5.Y = 107;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.tbVoltage = new global::Gtk.Entry ();
		this.tbVoltage.CanFocus = true;
		this.tbVoltage.Name = "tbVoltage";
		this.tbVoltage.IsEditable = true;
		this.tbVoltage.InvisibleChar = '●';
		this.fixed1.Add (this.tbVoltage);
		global::Gtk.Fixed.FixedChild w6 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.tbVoltage]));
		w6.X = 295;
		w6.Y = 139;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.textBox3 = new global::Gtk.Entry ();
		this.textBox3.CanFocus = true;
		this.textBox3.Name = "textBox3";
		this.textBox3.IsEditable = true;
		this.textBox3.InvisibleChar = '●';
		this.fixed1.Add (this.textBox3);
		global::Gtk.Fixed.FixedChild w7 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.textBox3]));
		w7.X = 294;
		w7.Y = 170;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.notebook1 = new global::Gtk.Notebook ();
		this.notebook1.CanFocus = true;
		this.notebook1.Name = "notebook1";
		this.notebook1.CurrentPage = 1;
		// Notebook tab
		global::Gtk.Label w8 = new global::Gtk.Label ();
		w8.Visible = true;
		this.notebook1.Add (w8);
		this.label1 = new global::Gtk.Label ();
		this.label1.Name = "label1";
		this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("page1");
		this.notebook1.SetTabLabel (w8, this.label1);
		this.label1.ShowAll ();
		// Notebook tab
		global::Gtk.Label w9 = new global::Gtk.Label ();
		w9.Visible = true;
		this.notebook1.Add (w9);
		this.label2 = new global::Gtk.Label ();
		this.label2.Name = "label2";
		this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("page2");
		this.notebook1.SetTabLabel (w9, this.label2);
		this.label2.ShowAll ();
		this.fixed1.Add (this.notebook1);
		global::Gtk.Fixed.FixedChild w10 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.notebook1]));
		w10.X = 25;
		w10.Y = 70;
		this.vpaned1.Add (this.fixed1);
		this.Add (this.vpaned1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 537;
		this.DefaultHeight = 461;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
	}
}