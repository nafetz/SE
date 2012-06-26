using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace LiftSimulation
{
    public partial class UserInterface : Form
    {

        #region Member

        private Defaults.MoreOrLess _passengersIO;

        private Label[] label_FloorNumbers;     //Anzeige der Etatennummern
        private Label[] label_CurrentPosition;  //Anzeige der aktuellen Etage (gleich in allen Labels)
        private GroupBox[] groupBox_Floors;     //GroupBoxen für die Etagen
        private PictureBox[] pictBox_DoorStates;
        private Image img_door1;
        private Image img_door2;
        private Image img_direction;
        private Button[] button_intern;
        private Button[] button_upward;
        private Button[] button_downward;
 

        #endregion

        #region Konstruktoren

        /// <summary>
        /// Konstruktor
        /// </summary>
        public UserInterface()
        {
            InitializeComponent();  //Intialisierung der statisch erzeugten Formularelemente
            ChangeDirection();            
            _passengersIO = Defaults.MoreOrLess.Neither;

            groupBox_Floors = new GroupBox[Defaults.Floors];
            label_FloorNumbers = new Label[ Defaults.Floors ];
            label_CurrentPosition = new Label[Defaults.Floors];            
            pictBox_DoorStates = new PictureBox[Defaults.Floors];
            button_intern = new Button[Defaults.Floors];
            button_upward = new Button[Defaults.Floors];
            button_downward = new Button[Defaults.Floors];

            img_door1 = Image.FromFile(Defaults.GetProjectPath() + @"\Pictures\Aufzugtueren_auf.gif");
            img_door2 = Image.FromFile(Defaults.GetProjectPath() + @"\Pictures\Aufzugtueren_zu.gif");

            for (int i = Defaults.Floors -1 ; i >= 0; i--)
            {
                groupBox_Floors[i] = new GroupBox();
                groupBox_Floors[i].Text = "";
                groupBox_Floors[i].Location = new Point(30, Defaults.Floors*100 - 110*i );
                groupBox_Floors[i].Height = 110;
                groupBox_Floors[i].Width = 350;
                groupBox_Floors[i].Name = "groupBox_floor" + i;
                groupBox_outsite.Controls.Add(groupBox_Floors[i]);


                label_FloorNumbers[ i ] = new Label();
                switch (i)
                {
                    case 0: label_FloorNumbers[ i ].Text = "1. UG"; break;
                    case 1: label_FloorNumbers[ i ].Text = "EG"; break;
                    default: label_FloorNumbers[ i ].Text = ( i - 1 ) + ". OG"; break;
                }
                label_FloorNumbers[ i ].Width = 70;
                label_FloorNumbers[i].Name = "label_floor" + i;
                groupBox_Floors[ i ].Controls.Add( label_FloorNumbers[ i ] );
                label_FloorNumbers[ i ].Font = new System.Drawing.Font( "Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );

                pictBox_DoorStates[i] = new PictureBox();
                pictBox_DoorStates[i].Width = 30;
                pictBox_DoorStates[i].Height = 30;
                pictBox_DoorStates[i].Location = new Point(100, 50);
                pictBox_DoorStates[i].Name = "pictureBox_doorstate" + i;
                pictBox_DoorStates[i].Image = img_door2;
                groupBox_Floors[i].Controls.Add(pictBox_DoorStates[i]);
                
                label_CurrentPosition[i] = new Label();
                label_CurrentPosition[i].Text = "#Position";
                label_CurrentPosition[i].Name = "label_position" + i;
                label_CurrentPosition[i].Width = 60;
                label_CurrentPosition[i].Location = new Point(100, 12);
                groupBox_Floors[i].Controls.Add(label_CurrentPosition[i]);

                button_intern[i] = new Button();
                button_intern[i].Location = new Point(20, Defaults.Floors*45 - 45 * i);
                button_intern[i].Height = 40;
                button_intern[i].Width = 100;
                button_intern[i].Name = "Button_int"+i;
                switch (i){               
                    case 0: button_intern[i].Text = "1. UG"; break;
                    case 1: button_intern[i].Text = "EG"; break;
                    default: button_intern[i].Text = (i -1) + ". OG"; break;
                }
                groupBox_floor_selection.Controls.Add(button_intern[i]);
                button_intern[i].Click += new System.EventHandler(ClickInnerButton);

                if(i!=Defaults.Floors - 1){
                    button_upward[i] = new Button();
                    button_upward[i].Location = new Point(250, 25);
                    button_upward[i].Height = 30;
                    button_upward[i].Width = 70;
                    button_upward[i].Text = "Aufwärts";
                    button_upward[i].Name = "Button_up_" + i;
                    groupBox_Floors[i].Controls.Add(button_upward[i]);
                    button_upward[i].Click += new System.EventHandler(ClickOutsideButton);                   
                }
                
                if (i != 0)
                {
                    button_downward[i] = new Button();
                    button_downward[i].Location = new Point(250, 65);
                    button_downward[i].Height = 30;
                    button_downward[i].Width = 70;
                    button_downward[i].Text = "Abwärts";
                    button_downward[i].Name = "Button_up_" + i;
                    groupBox_Floors[i].Controls.Add(button_downward[i]);
                    button_downward[i].Click += new System.EventHandler(ClickOutsideButton);                 
                }                                      
            }// for-Schleife            
        }

        #endregion

        #region Properties

        /// <summary>
        /// Liste mit Aufwärtswünschen aus UI
        /// </summary>
        public List<bool> UpwardRequired
        {
            get
            {
                List<bool> _upwards = new List<bool>();
                for (int i = 0; i < Defaults.Floors; i++)
                {
                    if (i == Defaults.Floors - 1)
                    {
                        _upwards.Add(false);
                    }
                    else
                    {
                        if (button_upward[i].Enabled == false) _upwards.Add(true);
                        else _upwards.Add(false);
                    }
                }
                return _upwards;
            }// get

            set
            {       List<bool> _upwards = value; //kann man die Liste einfach so kopieren?
                    for (int i = 0; i < Defaults.Floors - 1; i++) //geht bis zum vorletzten, da es oben ohnehin kein "hoch" gibt
                    {
                        if (_upwards[i] == true) button_upward[i].Enabled = false;
                        else button_upward[i].Enabled = true;
                    
                    }               

            }

        }

        /// <summary>
        /// Liste mit Abwärtswünschen aus UI
        /// </summary>
        public List<bool> DownwardRequired
        {
            get
            {
               
                List<bool> _downwards = new List<bool>();
                for( int i = 0; i < Defaults.Floors; i++ )
                {
                    if( i == 0 )
                    {
                        _downwards.Add( false );
                    }
                    else
                    {
                        if( button_downward[i].Enabled == false ) _downwards.Add( true );
                        else _downwards.Add(false);
                    }
                }
                return _downwards;
            }// get
            set
            {
                List<bool> _downwards = value; //kann man die Liste einfach so kopieren?
                for (int i = 1; i < Defaults.Floors; i++) //startet bei 1, da es unten ohnehin kein "runter" gibt
                {
                    if (_downwards[i] == true) button_downward[i].Enabled = false;
                    else button_downward[i].Enabled = true;

                }  
            }
        }

        /// <summary>
        /// Liste der im Fahrstuhl geäußerten Wünsche aus UI
        /// </summary>
        public List<bool> InternRequired
        {
            get
            {
                List<bool> _interns = new List<bool>();

                for (int i = 0; i < Defaults.Floors; i++)
                {                   
                        if (button_intern[i].Enabled == false)
                        {
                            _interns.Add(true);
                        }
                        else
                        {
                            _interns.Add(false);
                        }
                    
                }
                               
                return _interns;
            }
            set
            {
                List<bool> _interns = value; 
                for (int i = 0 ; i <Defaults.Floors; i++) 
                {
                    if (_interns[i] == true) button_intern[i].Enabled = false;
                    else button_intern[i].Enabled = true;
                }
            }
        }

        /// <summary>
        /// Passagiere im Fahrstuhl für die Anzeige in der UI
        /// </summary>
        public int PassengersCount
        {
            set
            {
                int Count = value;

                label_display_passengers.Text = Count.ToString();
                if (Count > Defaults.MaximumPassengers)
                {
                    label_display_passengers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    label_display_passengers.ForeColor = Color.Red;
                }
                else
                {
                    label_display_passengers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    label_display_passengers.ForeColor = Color.Black;
                }    
            }
        }

        /// <summary>
        /// Wert zur Anzeige der aktuellen Position im Fahrstuhl
        /// </summary>
        public int CurrentPosition
        {
            set
            {
                int pos = Defaults.FloorToIdx(value);
                String position_value;
                if (pos > Defaults.Floors || pos < 0) return;
                switch (pos)
                {
                    case 0: position_value = "1. UG"; break;
                    case 1: position_value = "EG"; break;
                    default: position_value = pos-1 + ". OG"; break;
                }

                foreach (Label poslabel in label_CurrentPosition)
                {
                    poslabel.Text = position_value; //äußere Label
                }

                label_floor_display.Text = position_value; //Label im inneren

                for (int i = 0; i < Defaults.Floors; i++)
                {
                    if (i == pos)
                    {
                        groupBox_Floors[i].BackColor = Color.White;
                        //required[i].BackColor = Color.White;
                    }
                    else
                    {
                        groupBox_Floors[i].BackColor = System.Drawing.SystemColors.Control;
                        //required[i].BackColor = System.Drawing.SystemColors.Control;
                    }
                }
            }
        }

        /*  Kein Verweis gefunden
        /// <summary>
        /// Wert zur Anzeige der aktuellen Richtung
        /// Wird das überhaupt gebraucht?
        /// </summary>
        public Defaults.Direction Direction
        {
            set
            {
                Defaults.Direction direction = value;
                switch (direction)
                {
                    case Defaults.Direction.Downward:
                        {
                            pictureBox_direction.Hide();
                            // dein Code hier
                        } break;
                    case Defaults.Direction.Upward:
                        {
                            // und hier
                        } break;
                }
            }
        }
        */

        public Defaults.MoreOrLess PassengersIO
        {
            get { return _passengersIO; }
            set { _passengersIO = value; }
        }

        /// <summary>
        /// Anzeige der aktuellen Parameter im DataGridView
        /// </summary>
        public Defaults._logentry logging{
            set
            {

                int pos = dataGridView_log.RowCount + 1;
              /*  dataGridView_log.Rows.Add(pos.ToString(),
                                       value._direction.ToString(),
                                       value._floor.ToString(),
                                       value._passenger.ToString(),
                                       value._state.ToString()
                    );
                dataGridView_log.FirstDisplayedCell = dataGridView_log.Rows[dataGridView_log.RowCount - 1].Cells[0];
                */
            }
        }

        public Button PlusPassengersButton
        {
            get { return button_more_passenger; }
            set { button_more_passenger = value; }
        }

        public Button MinusPassengersButton
        {
            get { return button_less_passenger; }
            set { button_less_passenger = value; }
        }

        public Timer Doortimer
        {

            get { return timer_tuer_zu; }
            set { timer_tuer_zu = value; }

        }

        public Timer Movetimer
        {
            get { return timer_fahren; }
            set { timer_fahren = value; }
        }

        #endregion

        #region Methoden

        /// <summary>
        /// Tür öffnen auf auf der GUI darstellen
        /// </summary>
        /// <param name="floor"></param>
        public void open_door(int floor)
        {
            if (floor < 0 || floor > Defaults.Floors) return;
            //if (doorstates[floor].Image == img_door2)
                pictBox_DoorStates[floor].Image = img_door1;            
        }

        /// <summary>
        /// Tür schießen auf der GUI darstellen
        /// </summary>
        /// <param name="floor"></param>
        public void close_door(int floor)
        {
            if (floor < 0 || floor > Defaults.Floors) return;
            if (pictBox_DoorStates[floor].Image == img_door1) pictBox_DoorStates[floor].Image = img_door2;
        }

        public void ResetPassengerIO() 
        {
            _passengersIO = Defaults.MoreOrLess.Neither;
        }

        #region Button Clicks
        private void ClickInnerButton( object sender, EventArgs e )
        {
            Button currentButton = sender as Button;
            currentButton.Enabled = false;

            Syncronize.syncinnerWishes( Syncronize.To.Elevator );
            BusyCheck();
            // Syncronize.executeLoop();            
        }

        private void ClickOutsideButton( object sender, EventArgs e )
        {
            Button currentButt = sender as Button;
            currentButt.Enabled = false;

            Syncronize.syncDownwardWishes( Syncronize.To.Elevator );
            Syncronize.syncUpwardWishes( Syncronize.To.Elevator );
            BusyCheck();
            //Syncronize.executeLoop();
        }

        private void button_more_passenger_Click( object sender, EventArgs e ) //+1 Button
        {
            _passengersIO = Defaults.MoreOrLess.More;
            Syncronize.executeLoop();
            _passengersIO = Defaults.MoreOrLess.Neither;
            //Defaults.ManualResetEvent.Set();
            // Syncronize.DoorTimerReset();
        }

        private void button_less_passenger_Click( object sender, EventArgs e ) //-1 Button
        {
            _passengersIO = Defaults.MoreOrLess.Less;
            Syncronize.executeLoop();
            _passengersIO = Defaults.MoreOrLess.Neither;
            //Syncronize.DoorTimerReset();
            //Defaults.ManualResetEvent.Set();
        }

        private void button_emergency_Click( object sender, EventArgs e )
        {
            MessageBox.Show( "Notruf betätigt. Fahrstuhl kommt an der nächstbesten Etage zum Stillstand. Fahrwünsche werden gelöscht." );

            /********************************************
             *           das Logging fehlt              *
             ********************************************/           

            for( int i = 0; i < Defaults.Floors; i++ )
            {
                button_intern[ i ].Enabled = true;
                if( i != 0 ) button_downward[ i ].Enabled = true;
                if( i != Defaults.Floors - 1 ) button_upward[ i ].Enabled = true;
            }
            Syncronize.syncDownwardWishes( Syncronize.To.Elevator );
            Syncronize.syncinnerWishes( Syncronize.To.Elevator );
            Syncronize.syncUpwardWishes( Syncronize.To.Elevator );
        }

        private void button_open_door_Click( object sender, EventArgs e )
        {
            button_less_passenger.Enabled = true;
            button_more_passenger.Enabled = true;
            Syncronize.DoorTimerReset();
            Syncronize.SetState( Defaults.State.FixedOpen ); //ggf. überdenken
            open_door( Syncronize.syncFloor() );
        }

        private void button_close_door_Click( object sender, EventArgs e )
        {
            // Alles Entfernt, da der Button nur Placebo sein soll..

            //button_less_passenger.Enabled = false;
            //button_more_passenger.Enabled = false;
            //Syncronize.DoorTimerStop();
            //Syncronize.SetState(Defaults.State.FixedClosed);
        }
        #endregion

        public void ChangeDirection()
        {
            img_direction = pictureBox_direction.Image;
            img_direction.RotateFlip( RotateFlipType.Rotate180FlipX );
            pictureBox_direction.Image = img_direction;
        }

        public void BusyCheck()
        {
            if( Syncronize.TaskStatus == false )
            {
                Syncronize.TaskStatus = true;
                Syncronize.executeLoop();
            }
        }

        private void timer_tuer_zu_Tick( object sender, EventArgs e )
        {
            timer_tuer_zu.Stop();
            Syncronize.PassengerButtonsEnable( false );
            Syncronize.SetState( Defaults.State.FixedClosed );
        }

        private void timer_fahren_Tick( object sender, EventArgs e )
        {
            timer_fahren.Stop();
            pictureBox_direction.Visible = false;
            Syncronize.syncinnerWishes( Syncronize.To.UI );
            Syncronize.syncDownwardWishes( Syncronize.To.Elevator );
            Syncronize.syncUpwardWishes( Syncronize.To.Elevator );
            Syncronize.executeLoop();
        }

        public void show_direction() 
        { pictureBox_direction.Visible = true; }

        public void enableInnerButtons( bool value )
        {
            for( int i = 0; i < Defaults.Floors; i++ )
                button_intern[ i ].Enabled = value;
        }

        #endregion

    }
}
