namespace LiftSimulation
{
    partial class UserInterface
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( UserInterface ) );
            this.groupBox_outsite = new System.Windows.Forms.GroupBox();
            this.groupBox_inside = new System.Windows.Forms.GroupBox();
            this.dataGridView_log = new System.Windows.Forms.DataGridView();
            this.Eintrag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Richtung = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Etage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Passagierer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox_control_inside = new System.Windows.Forms.GroupBox();
            this.groupBox_passengers_control = new System.Windows.Forms.GroupBox();
            this.button_more_passenger = new System.Windows.Forms.Button();
            this.button_less_passenger = new System.Windows.Forms.Button();
            this.label_display_passengers = new System.Windows.Forms.Label();
            this.label_passengers = new System.Windows.Forms.Label();
            this.groupBox_door_control = new System.Windows.Forms.GroupBox();
            this.button_close_door = new System.Windows.Forms.Button();
            this.button_open_door = new System.Windows.Forms.Button();
            this.button_emergency = new System.Windows.Forms.Button();
            this.groupBox_floor_selection = new System.Windows.Forms.GroupBox();
            this.label_geschosswahl = new System.Windows.Forms.Label();
            this.groupBox_position_display = new System.Windows.Forms.GroupBox();
            this.pictureBox_direction = new System.Windows.Forms.PictureBox();
            this.label_floor_display = new System.Windows.Forms.Label();
            this.timer_tuer_zu = new System.Windows.Forms.Timer( this.components );
            this.timer_fahren = new System.Windows.Forms.Timer( this.components );
            this.groupBox_inside.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.dataGridView_log ) ).BeginInit();
            this.groupBox_control_inside.SuspendLayout();
            this.groupBox_passengers_control.SuspendLayout();
            this.groupBox_door_control.SuspendLayout();
            this.groupBox_floor_selection.SuspendLayout();
            this.groupBox_position_display.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.pictureBox_direction ) ).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_outsite
            // 
            this.groupBox_outsite.Location = new System.Drawing.Point( 10, 10 );
            this.groupBox_outsite.Name = "groupBox_outsite";
            this.groupBox_outsite.Size = new System.Drawing.Size( 450, 740 );
            this.groupBox_outsite.TabIndex = 0;
            this.groupBox_outsite.TabStop = false;
            this.groupBox_outsite.Text = "Lift-Steuerung außen";
            // 
            // groupBox_inside
            // 
            this.groupBox_inside.Controls.Add( this.dataGridView_log );
            this.groupBox_inside.Controls.Add( this.groupBox_control_inside );
            this.groupBox_inside.Location = new System.Drawing.Point( 480, 10 );
            this.groupBox_inside.Name = "groupBox_inside";
            this.groupBox_inside.Size = new System.Drawing.Size( 450, 740 );
            this.groupBox_inside.TabIndex = 1;
            this.groupBox_inside.TabStop = false;
            this.groupBox_inside.Text = "Lift-Steuerung innen";
            // 
            // dataGridView_log
            // 
            this.dataGridView_log.AllowUserToAddRows = false;
            this.dataGridView_log.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.NullValue = "-";
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_log.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_log.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_log.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_log.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_log.Columns.AddRange( new System.Windows.Forms.DataGridViewColumn[] {
            this.Eintrag,
            this.Richtung,
            this.Etage,
            this.Passagierer,
            this.Status} );
            this.dataGridView_log.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridView_log.Location = new System.Drawing.Point( 21, 572 );
            this.dataGridView_log.Margin = new System.Windows.Forms.Padding( 2 );
            this.dataGridView_log.Name = "dataGridView_log";
            this.dataGridView_log.Size = new System.Drawing.Size( 390, 153 );
            this.dataGridView_log.TabIndex = 1;
            // 
            // Eintrag
            // 
            this.Eintrag.FillWeight = 50F;
            this.Eintrag.HeaderText = "Eintrag";
            this.Eintrag.Name = "Eintrag";
            this.Eintrag.ReadOnly = true;
            this.Eintrag.Width = 50;
            // 
            // Richtung
            // 
            this.Richtung.FillWeight = 90F;
            this.Richtung.HeaderText = "Richtung";
            this.Richtung.Name = "Richtung";
            this.Richtung.ReadOnly = true;
            this.Richtung.Width = 60;
            // 
            // Etage
            // 
            this.Etage.HeaderText = "Etage";
            this.Etage.Name = "Etage";
            this.Etage.ReadOnly = true;
            this.Etage.Width = 60;
            // 
            // Passagierer
            // 
            this.Passagierer.HeaderText = "Passagierer";
            this.Passagierer.Name = "Passagierer";
            this.Passagierer.ReadOnly = true;
            this.Passagierer.Width = 75;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // groupBox_control_inside
            // 
            this.groupBox_control_inside.Controls.Add( this.groupBox_passengers_control );
            this.groupBox_control_inside.Controls.Add( this.groupBox_door_control );
            this.groupBox_control_inside.Controls.Add( this.groupBox_floor_selection );
            this.groupBox_control_inside.Controls.Add( this.groupBox_position_display );
            this.groupBox_control_inside.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox_control_inside.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox_control_inside.Location = new System.Drawing.Point( 45, 32 );
            this.groupBox_control_inside.Name = "groupBox_control_inside";
            this.groupBox_control_inside.Size = new System.Drawing.Size( 329, 521 );
            this.groupBox_control_inside.TabIndex = 0;
            this.groupBox_control_inside.TabStop = false;
            // 
            // groupBox_passengers_control
            // 
            this.groupBox_passengers_control.Controls.Add( this.button_more_passenger );
            this.groupBox_passengers_control.Controls.Add( this.button_less_passenger );
            this.groupBox_passengers_control.Controls.Add( this.label_display_passengers );
            this.groupBox_passengers_control.Controls.Add( this.label_passengers );
            this.groupBox_passengers_control.Location = new System.Drawing.Point( 28, 409 );
            this.groupBox_passengers_control.Name = "groupBox_passengers_control";
            this.groupBox_passengers_control.Size = new System.Drawing.Size( 283, 85 );
            this.groupBox_passengers_control.TabIndex = 3;
            this.groupBox_passengers_control.TabStop = false;
            // 
            // button_more_passenger
            // 
            this.button_more_passenger.Enabled = false;
            this.button_more_passenger.Location = new System.Drawing.Point( 176, 19 );
            this.button_more_passenger.Name = "button_more_passenger";
            this.button_more_passenger.Size = new System.Drawing.Size( 90, 25 );
            this.button_more_passenger.TabIndex = 3;
            this.button_more_passenger.Text = "+1 Passagier";
            this.button_more_passenger.UseVisualStyleBackColor = true;
            this.button_more_passenger.Click += new System.EventHandler( this.button_more_passenger_Click );
            // 
            // button_less_passenger
            // 
            this.button_less_passenger.Enabled = false;
            this.button_less_passenger.Location = new System.Drawing.Point( 176, 54 );
            this.button_less_passenger.Name = "button_less_passenger";
            this.button_less_passenger.Size = new System.Drawing.Size( 90, 25 );
            this.button_less_passenger.TabIndex = 2;
            this.button_less_passenger.Text = "-1 Passagier";
            this.button_less_passenger.UseVisualStyleBackColor = true;
            this.button_less_passenger.Click += new System.EventHandler( this.button_less_passenger_Click );
            // 
            // label_display_passengers
            // 
            this.label_display_passengers.AutoSize = true;
            this.label_display_passengers.Font = new System.Drawing.Font( "Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.label_display_passengers.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_display_passengers.Location = new System.Drawing.Point( 60, 54 );
            this.label_display_passengers.Name = "label_display_passengers";
            this.label_display_passengers.Size = new System.Drawing.Size( 23, 25 );
            this.label_display_passengers.TabIndex = 1;
            this.label_display_passengers.Text = "0";
            // 
            // label_passengers
            // 
            this.label_passengers.AutoSize = true;
            this.label_passengers.Location = new System.Drawing.Point( 49, 25 );
            this.label_passengers.Name = "label_passengers";
            this.label_passengers.Size = new System.Drawing.Size( 83, 17 );
            this.label_passengers.TabIndex = 0;
            this.label_passengers.Text = "Passagiere:";
            // 
            // groupBox_door_control
            // 
            this.groupBox_door_control.Controls.Add( this.button_close_door );
            this.groupBox_door_control.Controls.Add( this.button_open_door );
            this.groupBox_door_control.Controls.Add( this.button_emergency );
            this.groupBox_door_control.Location = new System.Drawing.Point( 186, 166 );
            this.groupBox_door_control.Name = "groupBox_door_control";
            this.groupBox_door_control.Size = new System.Drawing.Size( 120, 225 );
            this.groupBox_door_control.TabIndex = 2;
            this.groupBox_door_control.TabStop = false;
            // 
            // button_close_door
            // 
            this.button_close_door.Location = new System.Drawing.Point( 16, 116 );
            this.button_close_door.Name = "button_close_door";
            this.button_close_door.Size = new System.Drawing.Size( 87, 32 );
            this.button_close_door.TabIndex = 2;
            this.button_close_door.Text = "> <";
            this.button_close_door.UseVisualStyleBackColor = true;
            this.button_close_door.Click += new System.EventHandler( this.button_close_door_Click );
            // 
            // button_open_door
            // 
            this.button_open_door.Location = new System.Drawing.Point( 16, 78 );
            this.button_open_door.Name = "button_open_door";
            this.button_open_door.Size = new System.Drawing.Size( 87, 32 );
            this.button_open_door.TabIndex = 1;
            this.button_open_door.Text = "< >";
            this.button_open_door.UseVisualStyleBackColor = true;
            this.button_open_door.Click += new System.EventHandler( this.button_open_door_Click );
            // 
            // button_emergency
            // 
            this.button_emergency.Location = new System.Drawing.Point( 16, 41 );
            this.button_emergency.Name = "button_emergency";
            this.button_emergency.Size = new System.Drawing.Size( 87, 32 );
            this.button_emergency.TabIndex = 0;
            this.button_emergency.Text = "Notruf";
            this.button_emergency.UseVisualStyleBackColor = true;
            this.button_emergency.Click += new System.EventHandler( this.button_emergency_Click );
            // 
            // groupBox_floor_selection
            // 
            this.groupBox_floor_selection.Controls.Add( this.label_geschosswahl );
            this.groupBox_floor_selection.Location = new System.Drawing.Point( 28, 53 );
            this.groupBox_floor_selection.Name = "groupBox_floor_selection";
            this.groupBox_floor_selection.Size = new System.Drawing.Size( 133, 338 );
            this.groupBox_floor_selection.TabIndex = 1;
            this.groupBox_floor_selection.TabStop = false;
            // 
            // label_geschosswahl
            // 
            this.label_geschosswahl.AutoSize = true;
            this.label_geschosswahl.Font = new System.Drawing.Font( "Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.label_geschosswahl.Location = new System.Drawing.Point( 18, 26 );
            this.label_geschosswahl.Name = "label_geschosswahl";
            this.label_geschosswahl.Size = new System.Drawing.Size( 119, 20 );
            this.label_geschosswahl.TabIndex = 0;
            this.label_geschosswahl.Text = "Geschosswahl";
            // 
            // groupBox_position_display
            // 
            this.groupBox_position_display.Controls.Add( this.pictureBox_direction );
            this.groupBox_position_display.Controls.Add( this.label_floor_display );
            this.groupBox_position_display.Location = new System.Drawing.Point( 186, 53 );
            this.groupBox_position_display.Name = "groupBox_position_display";
            this.groupBox_position_display.Size = new System.Drawing.Size( 125, 104 );
            this.groupBox_position_display.TabIndex = 0;
            this.groupBox_position_display.TabStop = false;
            // 
            // pictureBox_direction
            // 
            this.pictureBox_direction.ErrorImage = null;
            this.pictureBox_direction.Image = ( (System.Drawing.Image)( resources.GetObject( "pictureBox_direction.Image" ) ) );
            this.pictureBox_direction.Location = new System.Drawing.Point( 50, 55 );
            this.pictureBox_direction.Name = "pictureBox_direction";
            this.pictureBox_direction.Size = new System.Drawing.Size( 32, 32 );
            this.pictureBox_direction.TabIndex = 3;
            this.pictureBox_direction.TabStop = false;
            this.pictureBox_direction.Visible = false;
            // 
            // label_floor_display
            // 
            this.label_floor_display.AutoSize = true;
            this.label_floor_display.Font = new System.Drawing.Font( "Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.label_floor_display.Location = new System.Drawing.Point( 44, 16 );
            this.label_floor_display.Name = "label_floor_display";
            this.label_floor_display.Size = new System.Drawing.Size( 49, 29 );
            this.label_floor_display.TabIndex = 0;
            this.label_floor_display.Text = "EG";
            // 
            // timer_tuer_zu
            // 
            this.timer_tuer_zu.Interval = 5000;
            this.timer_tuer_zu.Tick += new System.EventHandler( this.timer_tuer_zu_Tick );
            // 
            // timer_fahren
            // 
            this.timer_fahren.Interval = 2000;
            this.timer_fahren.Tick += new System.EventHandler( this.timer_fahren_Tick );
            // 
            // UserInterface
            // 
            this.ClientSize = new System.Drawing.Size( 982, 755 );
            this.Controls.Add( this.groupBox_inside );
            this.Controls.Add( this.groupBox_outsite );
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size( 1000, 800 );
            this.MinimumSize = new System.Drawing.Size( 1000, 800 );
            this.Name = "UserInterface";
            this.Text = "Lift - Simulation";
            this.groupBox_inside.ResumeLayout( false );
            ( (System.ComponentModel.ISupportInitialize)( this.dataGridView_log ) ).EndInit();
            this.groupBox_control_inside.ResumeLayout( false );
            this.groupBox_passengers_control.ResumeLayout( false );
            this.groupBox_passengers_control.PerformLayout();
            this.groupBox_door_control.ResumeLayout( false );
            this.groupBox_floor_selection.ResumeLayout( false );
            this.groupBox_floor_selection.PerformLayout();
            this.groupBox_position_display.ResumeLayout( false );
            this.groupBox_position_display.PerformLayout();
            ( (System.ComponentModel.ISupportInitialize)( this.pictureBox_direction ) ).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_outsite;
        private System.Windows.Forms.GroupBox groupBox_inside;
        private System.Windows.Forms.GroupBox groupBox_control_inside;
        private System.Windows.Forms.GroupBox groupBox_door_control;
        private System.Windows.Forms.Button button_close_door;
        private System.Windows.Forms.Button button_open_door;
        private System.Windows.Forms.Button button_emergency;
        private System.Windows.Forms.GroupBox groupBox_floor_selection;
        private System.Windows.Forms.GroupBox groupBox_position_display;
        private System.Windows.Forms.Label label_floor_display;
        private System.Windows.Forms.Label label_geschosswahl;
        private System.Windows.Forms.GroupBox groupBox_passengers_control;
        private System.Windows.Forms.Button button_more_passenger;
        private System.Windows.Forms.Button button_less_passenger;
        private System.Windows.Forms.Label label_display_passengers;
        private System.Windows.Forms.Label label_passengers;
        private System.Windows.Forms.PictureBox pictureBox_direction;
        private System.Windows.Forms.DataGridView dataGridView_log;
        private System.Windows.Forms.DataGridViewTextBoxColumn Eintrag;
        private System.Windows.Forms.DataGridViewTextBoxColumn Richtung;
        private System.Windows.Forms.DataGridViewTextBoxColumn Etage;
        private System.Windows.Forms.DataGridViewTextBoxColumn Passagierer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.Timer timer_tuer_zu;
        private System.Windows.Forms.Timer timer_fahren;



    }
}

