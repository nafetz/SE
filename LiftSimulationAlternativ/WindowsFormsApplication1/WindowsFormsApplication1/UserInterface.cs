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

        private Label[] label_floor_numbers; //Anzeige der Etatennummern
        private Label[] label_current_position; //Anzeige der aktuellen Etage (gleich in allen Labels)
        private GroupBox[] GroupBox_floors;    //GroupBoxen für die Etagen
        private PictureBox[] PictureBox_doorstates;
        private Image image_door1;
        private Image image_door2;
        private Image image_direction;
        private Button[] button_intern;
        private Button[] button_upward;
        private Button[] button_downward;

        private List<bool> intern_requireds;
        private List<bool> downwards_requireds;
        private List<bool> upwards_requireds;
        private int floor;
        private int passengers;
        private bool busy;
        private Defaults.Direction current_direction;

        private Defaults.Door door;

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

            GroupBox_floors = new GroupBox[Defaults.Floors];
            label_floor_numbers = new Label[Defaults.Floors];
            label_current_position = new Label[Defaults.Floors];            
            PictureBox_doorstates = new PictureBox[Defaults.Floors];
            button_intern = new Button[Defaults.Floors];
            button_upward = new Button[Defaults.Floors];
            button_downward = new Button[Defaults.Floors];


            image_door1 = Image.FromFile(Defaults.GetProjectPath() + @"\Pictures\Aufzugtueren_auf.gif");
            image_door2 = Image.FromFile(Defaults.GetProjectPath() + @"\Pictures\Aufzugtueren_zu.gif");
            current_direction = Defaults.Direction.Upward;
            floor = 0;
            passengers = 0;

            door = Defaults.Door.Closed;
            busy = false;
            intern_requireds = new List<bool>();
            upwards_requireds = new List<bool>();
            downwards_requireds = new List<bool>();



            for (int i = Defaults.Floors -1 ; i >= 0; i--)
            {
                GroupBox_floors[i] = new GroupBox();
                GroupBox_floors[i].Text = "";
                GroupBox_floors[i].Location = new Point(30, Defaults.Floors*100 - 110*i );
                GroupBox_floors[i].Height = 110;
                GroupBox_floors[i].Width = 350;
                GroupBox_floors[i].Name = "groupBox_floor" + i;
                groupBox_outsite.Controls.Add(GroupBox_floors[i]);

                
                label_floor_numbers[i] = new Label();
                switch (i){               
                    case 0: label_floor_numbers[i].Text = "1. UG"; break;
                    case 1: label_floor_numbers[i].Text = "EG"; break;
                    default: label_floor_numbers[i].Text = (i -1) + ". OG"; break;
                }
                label_floor_numbers[i].Width = 70;
                label_floor_numbers[i].Location = new Point(10,10);
                label_floor_numbers[i].Name = "label_floor" + i;
                GroupBox_floors[i].Controls.Add(label_floor_numbers[i]);
                label_floor_numbers[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                PictureBox_doorstates[i] = new PictureBox();
                PictureBox_doorstates[i].Width = 30;
                PictureBox_doorstates[i].Height = 30;
                PictureBox_doorstates[i].Location = new Point(100, 50);
                PictureBox_doorstates[i].Name = "pictureBox_doorstate" + i;
                PictureBox_doorstates[i].Image = image_door2;
                GroupBox_floors[i].Controls.Add(PictureBox_doorstates[i]);
                
                label_current_position[i] = new Label();
                label_current_position[i].Text = "#Position";
                label_current_position[i].Name = "label_position" + i;
                label_current_position[i].Width = 60;
                label_current_position[i].Location = new Point(100, 12);
                GroupBox_floors[i].Controls.Add(label_current_position[i]);


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
                    GroupBox_floors[i].Controls.Add(button_upward[i]);
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
                    GroupBox_floors[i].Controls.Add(button_downward[i]);
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
            {       List<bool> _upwards = value; 
                    for (int i = 0; i < Defaults.Floors - 1; i++) 
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

                foreach (Label poslabel in label_current_position)
                {
                    poslabel.Text = position_value; //äußere Label
                }

                label_floor_display.Text = position_value; //Label im inneren

                for (int i = 0; i < Defaults.Floors; i++)
                {
                    if (i == pos)
                    {
                        GroupBox_floors[i].BackColor = Color.Yellow;                       
                    }
                    else
                    {
                        GroupBox_floors[i].BackColor = System.Drawing.SystemColors.Control;   
                    }
                }
            }
        }

        #endregion

        #region Methoden

        

        /// <summary>
        /// Tür öffnen auf auf der GUI darstellen
        /// </summary>
        /// <param name="floor"></param>
        public void gui_open_door()
        {
            if (floor < 0 || floor > Defaults.Floors) return;
            //if (doorstates[floor].Image == img_door2)
                PictureBox_doorstates[Defaults.FloorToIdx(floor)].Image = image_door1;
            
        }

        /// <summary>
        /// Tür schießen auf der GUI darstellen
        /// </summary>
        /// <param name="floor"></param>
        public void gui_close_door()
        {
            if (floor < 0 || floor > Defaults.Floors) return;
           // if (PictureBox_doorstates[floor].Image == image_door1) 
            PictureBox_doorstates[Defaults.FloorToIdx(floor)].Image = image_door2;

        }


        public void openDoor()
        {
            door = Defaults.Door.Open;
            button_less_passenger.Enabled = true;
            button_more_passenger.Enabled = true;

            // Bild auf GUI umschalten
            gui_open_door();
            DeleteReqired();

            timer_tuer_zu.Stop();
            timer_tuer_zu.Start();
        }

        public void closeDoor()
        {
            door = Defaults.Door.Closed;
            button_less_passenger.Enabled = false;
            button_more_passenger.Enabled = false;
            gui_close_door();

            // Bild auf GUI umschalten

            timer_tuer_zu.Stop();
        }

        public void go()
        {

            if (wishesHere() == true)
            {
                openDoor();
                return;
            }

            if (DirectionwishesInDirection() == true)
            {
                floorchange();
                timer_fahren.Start();
                return;
            }

            if(OppostieWishesInDirection()==true)
            {
                floorchange();
                timer_fahren.Start();
                return;
            }

            if (wishesHereInOppositeDirection())
            {
                switchDirection();
                openDoor();
                return;
            }

            if (OppostieWishesInOppositeDirection())
            {
                switchDirection();
                floorchange();
                timer_fahren.Start();
                return;
            }

            if (DirectionWishesInOppositeDirection())
            {
                switchDirection();
                floorchange();
                timer_fahren.Start();
                return;
            }

          

            busy = true;
        }

        public void floorchange()
        {
            switch (current_direction)
            {
                case Defaults.Direction.Upward: floor++; break;
                case Defaults.Direction.Downward: floor--; break;
            }

            if ((floor == 0 && current_direction == Defaults.Direction.Downward) || 
                (floor == Defaults.FloorToIdx(floor) && current_direction == Defaults.Direction.Upward))
            {
                switchDirection();
            }

            pictureBox_direction.Visible = true;

           
        }

        public bool highesorlowestfloor()
        {
            if (floor < 1 || floor == Defaults.Floors) return true;
            return false;
        }

        public void switchDirection()
        {
            switch(current_direction){
                case Defaults.Direction.Upward: current_direction = Defaults.Direction.Downward; break;
                case Defaults.Direction.Downward: current_direction = Defaults.Direction.Upward; break;
            }
        }


        /// <summary>
        /// Gibt es auf der aktuellen Etage innere Wünsche oder äußere Wünsche in Fahrtrichtung
        /// </summary>
        /// <returns>true or false</returns>
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

        /// <summary>
        /// Gibt es auf der aktuellen Etage äußere Wünsche entgegen Fahrtrichtung
        /// </summary>
        /// <returns>true or false</returns>

        public bool wishesHereInOppositeDirection()
        {
            switch (current_direction)
            {
                case Defaults.Direction.Upward:
                    {
                        if (downwards_requireds[Defaults.FloorToIdx(floor)] == true)
                            return true;
                    } break;
                case Defaults.Direction.Downward:
                    {
                        if (upwards_requireds[Defaults.FloorToIdx(floor)] == true)
                            return true;
                    } break;
            }
            return false;
        }

        /// <summary>
        /// Gibt auf einer Etage in Fahrtrichtung Wünsche von innen oder von außen in Fahrtrichtung?
        /// </summary>
        /// <returns>true or false</returns>

        public bool DirectionwishesInDirection()
        {
            switch (current_direction)
            {
                case Defaults.Direction.Upward:
                    {
                        for (int IDX = Defaults.FloorToIdx(floor); IDX < Defaults.FloorToIdx(Defaults.Floors)-1; IDX++)
                        {
                            if (upwards_requireds[IDX] == true || intern_requireds[IDX] == true)
                                return true;
                        }
                    }break;

                case Defaults.Direction.Downward:
                    {
                        for (int IDX = 0 ; IDX < Defaults.FloorToIdx(floor)-1; IDX++)
                        {
                            if (intern_requireds[IDX] == true || downwards_requireds[IDX] == true)
                                return true;
                        }
                    } break;
            }
            return false;
        }

        /// <summary>
        /// Gibt auf einer Etage in Fahrtrichtung Wünsche von außen entgegen der Fahrtrichtung?
        /// </summary>
        /// <returns>true or false</returns>
       
        public bool OppostieWishesInDirection()
        {
            switch (current_direction)
            {
                case Defaults.Direction.Upward:
                    {
                        for (int IDX = Defaults.FloorToIdx(floor); IDX < Defaults.FloorToIdx(Defaults.Floors)-1; IDX++)
                        {
                            if (downwards_requireds[IDX] == true)
                                return true;
                        }
                    } break;

                case Defaults.Direction.Downward:
                    {
                        for (int IDX = 0; IDX < Defaults.FloorToIdx(floor)-1; IDX++)
                        {
                            if (upwards_requireds[IDX] == true)
                                return true;
                        }
                    } break;
            }
            return false;
        }

        /// <summary>
        /// Gibt auf einer Etage entgegen Fahrtrichtung Wünsche von inner oer von außen entgegen der Fahrtrichtung?
        /// </summary>
        /// <returns>true or false</returns>

        public bool OppostieWishesInOppositeDirection()
        {
            switch (current_direction)
            {
                case Defaults.Direction.Upward:
                    {
                        for (int IDX = 0; IDX < Defaults.FloorToIdx(floor); IDX++)
                        {
                            if (downwards_requireds[IDX] == true || intern_requireds[IDX] == true)
                                return true;
                        }
                    } break;

                case Defaults.Direction.Downward:
                    {
                        for (int IDX = Defaults.FloorToIdx(Defaults.Floors)-Defaults.Basements-1; IDX > Defaults.FloorToIdx(floor); IDX--)
                        {
                            if (upwards_requireds[IDX] == true)
                                return true;
                        }
                    } break;
            }
            return false;
        }

        /// <summary>
        /// Gibt auf einer Etage entgegen der Fahrtrichtung Wünsche von außen in Fahrtrichtung?
        /// </summary>
        /// <returns>true or false</returns>

        public bool DirectionWishesInOppositeDirection()
        {
            switch (current_direction)
            {
                case Defaults.Direction.Upward:
                    {
                        for (int IDX = 0; IDX < Defaults.FloorToIdx(floor); IDX++)
                        {
                            if (upwards_requireds[IDX] == true)
                                return true;
                        }
                    } break;

                case Defaults.Direction.Downward:
                    {
                        for (int IDX = Defaults.FloorToIdx(Defaults.Floors)-Defaults.Basements-1; IDX > Defaults.FloorToIdx(floor); IDX--)
                        {
                            if (downwards_requireds[IDX] == true)
                                return true;
                        }
                    } break;
            }
            return false;
        }

        /// <summary>
        /// Öffnen sich die Türen auf einer Etage werden alle inneren und äußeren Wünsche gelöscht
        /// </summary>
        public void DeleteReqired()
        {

            if(floor < Defaults.FloorToIdx(Defaults.Floors))
            {
                 upwards_requireds[Defaults.FloorToIdx(floor)] = false;
                 button_upward[Defaults.FloorToIdx(floor)].Enabled = true;
             }

            if (floor > 0)
            {
                downwards_requireds[Defaults.FloorToIdx(floor)] = false;
                button_downward[Defaults.FloorToIdx(floor)].Enabled = true;

            }

          intern_requireds[Defaults.FloorToIdx(floor)] = false;
          button_intern[Defaults.FloorToIdx(floor)].Enabled = true;
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
           // passengers++;
            PassengersCount = ++passengers;

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
                //passengers--;
                PassengersCount = --passengers;
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

        private void ClickInnerButton(object sender, EventArgs e)
        {
            Button currentButton = sender as Button;
            currentButton.Enabled = false;
            intern_requireds = InternRequired;
            //Syncronize.syncinnerWishes(Syncronize.To.Elevator);
            if (busy) go();
            // Syncronize.executeLoop();            
        }

        private void ClickOutsideButton(object sender, EventArgs e)
        {
            Button currentButt = sender as Button;
            currentButt.Enabled = false;
            upwards_requireds = UpwardRequired;
            downwards_requireds = DownwardRequired;

            //Syncronize.syncDownwardWishes(Syncronize.To.Elevator);
            //Syncronize.syncUpwardWishes(Syncronize.To.Elevator);
            if (busy) go();
            //Syncronize.executeLoop();
        }
        #endregion


        public void ChangeDirection()
        {

            image_direction = pictureBox_direction.Image;
            image_direction.RotateFlip(RotateFlipType.Rotate180FlipX);
            pictureBox_direction.Image = image_direction;
               

        }


        private void timer_tuer_zu_Tick(object sender, EventArgs e)
        {
            timer_tuer_zu.Stop();
            closeDoor();

            //Move-Methode
            go();

            //Syncronize.PassengerButtonsEnable(false);
           // Syncronize.SetState(Defaults.State.FixedClosed);
            
        }



        private void timer_fahren_Tick(object sender, EventArgs e)
        {
           timer_fahren.Stop(); 
           pictureBox_direction.Visible = false;
           CurrentPosition = floor;
           go();
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
