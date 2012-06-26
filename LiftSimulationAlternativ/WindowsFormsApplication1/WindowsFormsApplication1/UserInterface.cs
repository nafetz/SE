using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace LiftSimulationAlternativ
{
    public partial class UserInterface : Form
    {

        #region Member

        //private Defaults.MoreOrLess _passengersIO;

        private Label[] floor_numbers; //Anzeige der Etatennummern
        private Label[] current_position; //Anzeige der aktuellen Etage (gleich in allen Labels)
        private GroupBox[] floors;    //GroupBoxen für die Etagen
        private PictureBox[] doorstates;
        private Image img_door1;
        private Image img_door2;
        private Image img_direction;
        private Button[] button_intern;
        private Button[] button_upward;
        private Button[] button_downward;

        private List<bool> intern_requireds;
        private List<bool> downwards_requireds;
        private List<bool> upwards_requireds;
        private int floor;
        private int passengers;
        private Defaults.Direction current_direction;

        private Defaults.Door door;

        private bool busy;
        #endregion

        #region Konstruktoren

        /// <summary>
        /// Konstruktor
        /// </summary>
        public UserInterface()
        {
            InitializeComponent(); //Intialisierung der statisch erzeugten Formularelemente
            ChangeDirection();            
            //_passengersIO = Defaults.MoreOrLess.Neither;

            floors = new GroupBox[Defaults.Floors];
            floor_numbers = new Label[Defaults.Floors];
            current_position = new Label[Defaults.Floors];            
            doorstates = new PictureBox[Defaults.Floors];
            button_intern = new Button[Defaults.Floors];
            button_upward = new Button[Defaults.Floors];
            button_downward = new Button[Defaults.Floors];


            //img_door1 = Image.FromFile(Defaults.GetProjectPath() + @"\Pictures\Aufzugtueren_auf.gif");
            //img_door2 = Image.FromFile(Defaults.GetProjectPath() + @"\Pictures\Aufzugtueren_zu.gif");
            current_direction = Defaults.Direction.Upward;
            floor = 0;
            passengers = 0;

            door = Defaults.Door.Closed;
            busy = false;


            for (int i = Defaults.Floors -1 ; i >= 0; i--)
            {
                floors[i] = new GroupBox();
                floors[i].Text = "";
                floors[i].Location = new Point(30, Defaults.Floors*100 - 110*i );
                floors[i].Height = 110;
                floors[i].Width = 350;
                floors[i].Name = "groupBox_floor" + i;
                groupBox_outsite.Controls.Add(floors[i]);

                
                floor_numbers[i] = new Label();
                switch (i){               
                    case 0: floor_numbers[i].Text = "1. UG"; break;
                    case 1: floor_numbers[i].Text = "EG"; break;
                    default: floor_numbers[i].Text = (i -1) + ". OG"; break;
                }
                floor_numbers[i].Width = 70;
                floor_numbers[i].Location = new Point(10,10);
                floor_numbers[i].Name = "label_floor" + i;
                floors[i].Controls.Add(floor_numbers[i]);
                floor_numbers[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                doorstates[i] = new PictureBox();
                doorstates[i].Width = 30;
                doorstates[i].Height = 30;
                doorstates[i].Location = new Point(100, 50);
                doorstates[i].Name = "pictureBox_doorstate" + i;
                doorstates[i].Image = img_door2;
                floors[i].Controls.Add(doorstates[i]);
                
                current_position[i] = new Label();
                current_position[i].Text = "#Position";
                current_position[i].Name = "label_position" + i;
                current_position[i].Width = 60;
                current_position[i].Location = new Point(100, 12);
                floors[i].Controls.Add(current_position[i]);


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
                    floors[i].Controls.Add(button_upward[i]);
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
                    floors[i].Controls.Add(button_downward[i]);
                    button_downward[i].Click += new System.EventHandler(ClickOutsideButton);
                 
                }

                intern_requireds.Add(false);
                upwards_requireds.Add(false);
                downwards_requireds.Add(false);
            }// Ende der for-Schleife       
            
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

                foreach (Label poslabel in current_position)
                {
                    poslabel.Text = position_value; //äußere Label
                }

                label_floor_display.Text = position_value; //Label im inneren

                for (int i = 0; i < Defaults.Floors; i++)
                {
                    if (i == pos)
                    {
                        floors[i].BackColor = Color.White;
                        //required[i].BackColor = Color.White;
                    }
                    else
                    {
                        floors[i].BackColor = System.Drawing.SystemColors.Control;
                        //required[i].BackColor = System.Drawing.SystemColors.Control;
                    }
                }
            }
        }

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
                dataGridView_log.Rows.Add(pos.ToString(),
                                       value._direction.ToString(),
                                       value._floor.ToString(),
                                       value._passenger.ToString(),
                                       value._state.ToString()
                    );
                dataGridView_log.FirstDisplayedCell = dataGridView_log.Rows[dataGridView_log.RowCount - 1].Cells[0];
                }
        }

        //public Button PlusPassengersButton
        //{
        //    get { return button_more_passenger; }
        //    set { button_more_passenger = value; }
        //}

        //public Button MinusPassengersButton
        //{
        //    get { return button_less_passenger; }
        //    set { button_less_passenger = value; }
        //}

        //public Timer Doortimer
        //{

        //    get { return timer_tuer_zu; }
        //    set { timer_tuer_zu = value; }

        //}

        //public Timer Movetimer
        //{
        //    get { return timer_fahren; }
        //    set { timer_fahren = value; }
        //}

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
                doorstates[floor].Image = img_door1;
            
        }

        /// <summary>
        /// Tür schießen auf der GUI darstellen
        /// </summary>
        /// <param name="floor"></param>
        public void close_door(int floor)
        {
            if (floor < 0 || floor > Defaults.Floors) return;
            if (doorstates[floor].Image == img_door1) doorstates[floor].Image = img_door2;

        }

        //public void ResetPassengerIO() 
        //{
        //    _passengersIO = Defaults.MoreOrLess.Neither;
        //}

        public void openDoor()
        {
            door = Defaults.Door.Open;
            button_less_passenger.Enabled = true;
            button_more_passenger.Enabled = true;

            // Bild auf GUI umschalten

            timer_tuer_zu.Stop();
            timer_tuer_zu.Start();
        }

        public void closeDoor()
        {
            door = Defaults.Door.Closed;
            button_less_passenger.Enabled = false;
            button_more_passenger.Enabled = false;

            // Bild auf GUI umschalten

            timer_tuer_zu.Stop();
        }

        public void go()
        {
            if (
                //upwards_requireds[Defaults.FloorToIdx(floor)] == true ||
                //downwards_requireds[Defaults.FloorToIdx(floor)] == true ||
                //intern_requireds[Defaults.FloorToIdx(floor)] == true
                )
            {
                openDoor();
            }
 
        }

        public bool wishesHere()
        {
            if (intern_requireds[Defaults.FloorToIdx(floor)] == true)
                return true;
            switch (current_direction)
            {
                case Defaults.Direction.Upward:
                    {
                        if(upwards_requireds[Defaults.FloorToIdx(floor)] == true)
                            return true;
                    } break;
                case Defaults.Direction.Downward:
                    {
                        if (downwards_requireds[Defaults.FloorToIdx(floor)] == true)
                            return true;
                    } break;
            }
            return false;
        }

        public bool wishesInDirection()
        {
            switch (current_direction)
            {
                case Defaults.Direction.Upward:
                    {
                        for (int IDX = Defaults.FloorToIdx(floor); IDX < Defaults.FloorToIdx(Defaults.Floors); IDX++)
                        {
                            if (upwards_requireds[IDX] == true || intern_requireds[IDX] == true || downwards_requireds[IDX] == true)
                                return true;
                        }
                    }break;

                case Defaults.Direction.Downward:
                    {
                        for (int IDX = 0 ; IDX < Defaults.FloorToIdx(floor); IDX++)
                        {
                            if (upwards_requireds[IDX] == true || intern_requireds[IDX] == true || downwards_requireds[IDX] == true)
                                return true;
                        }
                    } break;
            }
            return false;
        }
        #endregion


        #region Button Clicks
        private void button_more_passenger_Click(object sender, EventArgs e) //+1 Button
        {
           // _passengersIO = Defaults.MoreOrLess.More;
           //  //Syncronize.executeLoop();
           // _passengersIO = Defaults.MoreOrLess.Neither;
           // //Defaults.ManualResetEvent.Set();
           //// Syncronize.DoorTimerReset();
            passengers++;
            label_passengers.Text = passengers.ToString();

            if (passengers > 0)
            {
                button_less_passenger.Enabled = true;
            }
        }

        private void button_less_passenger_Click(object sender, EventArgs e) //-1 Button
        {
            //_passengersIO = Defaults.MoreOrLess.Less;
            //Syncronize.executeLoop();
            //_passengersIO = Defaults.MoreOrLess.Neither;
            //Syncronize.DoorTimerReset();
            //Defaults.ManualResetEvent.Set();
            if (passengers > 0)
            {
                passengers--;
                label_passengers.Text = passengers.ToString();
            }
            if (passengers == 0)
            {
                button_less_passenger.Enabled = false;
            }
        }

        private void button_emergency_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Notruf betätigt. Fahrstuhl kommt an der nächstbesten Etage zum Stillstand. Fahrwünsche werden gelöscht.");
            for (int i = 0; i < Defaults.Floors; i++ )
            {
                intern_requireds[i] = true;
                if (i != 0) downwards_requireds[i] = true;
                if (i != Defaults.Floors - 1) upwards_requireds[i] = true;
            }

            DownwardRequired = downwards_requireds;
            UpwardRequired = upwards_requireds;
            InternRequired = intern_requireds;

            //Syncronize.syncDownwardWishes(Syncronize.To.Elevator);
            //Syncronize.syncinnerWishes(Syncronize.To.Elevator);
            //Syncronize.syncUpwardWishes(Syncronize.To.Elevator);
        }

        private void button_open_door_Click(object sender, EventArgs e)
        {
            //Syncronize.DoorTimerReset();
            //Syncronize.SetState(Defaults.State.FixedOpen); //ggf. überdenken
            //open_door(Syncronize.syncFloor()); 
            openDoor();
        }

        private void button_close_door_Click(object sender, EventArgs e)
        {
            //button_less_passenger.Enabled = false;
            //button_more_passenger.Enabled = false;
            //Syncronize.DoorTimerStop();
            //Syncronize.SetState(Defaults.State.FixedClosed);
            closeDoor();
        }
        #endregion


        public void ChangeDirection()
        {

            img_direction = pictureBox_direction.Image;
            img_direction.RotateFlip(RotateFlipType.Rotate180FlipX);
            pictureBox_direction.Image = img_direction;
               

        }

        public void BusyCheck()
        {
            //if (Syncronize.TaskStatus == false)
            //{
            //    Syncronize.TaskStatus = true;
            //    Syncronize.executeLoop();
            //}
        }

        private void timer_tuer_zu_Tick(object sender, EventArgs e)
        {
            timer_tuer_zu.Stop();
            closeDoor();

            //Move-Methode

            //Syncronize.PassengerButtonsEnable(false);
           // Syncronize.SetState(Defaults.State.FixedClosed);
            
        }

        private void ClickInnerButton(object sender, EventArgs e)
        {
           Button currentButton = sender as Button;           
           currentButton.Enabled = false;           
           
           //Syncronize.syncinnerWishes(Syncronize.To.Elevator);
           BusyCheck();
          // Syncronize.executeLoop();            
        }

        private void ClickOutsideButton(object sender, EventArgs e)
        {
           Button currentButt = sender as Button;
           currentButt.Enabled = false;

           //Syncronize.syncDownwardWishes(Syncronize.To.Elevator);
           //Syncronize.syncUpwardWishes(Syncronize.To.Elevator);
           BusyCheck();
           //Syncronize.executeLoop();
        }

        private void timer_fahren_Tick(object sender, EventArgs e)
        {
           timer_fahren.Stop(); 
           pictureBox_direction.Visible = false;
           //Syncronize.syncinnerWishes(Syncronize.To.UI);
           //Syncronize.syncDownwardWishes(Syncronize.To.Elevator);
           //Syncronize.syncUpwardWishes(Syncronize.To.Elevator);
           //Syncronize.executeLoop();
          
        }

        public void show_direction()
        {
            pictureBox_direction.Visible = true;         
        }

        public void enableInnerButtons(bool value){
            for (int i = 0; i < Defaults.Floors; i++)                       
                 button_intern[i].Enabled = value;            
         }
    }
}
