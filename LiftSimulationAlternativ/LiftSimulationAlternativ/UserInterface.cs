using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace Alternativ
{
    public partial class UserInterface : Form
    {

        #region Member


        private Label[] label_floor_numbers; //Labels für die Anzeige der Etatennummern
        private Label[] label_current_position; //Labels für die Anzeige der aktuellen Etage (gleicher Text in allen Labels dieses Arrays )
        private GroupBox[] GroupBox_floors;    //GroupBoxen für die Etagen
        private PictureBox[] PictureBox_doorstates; //Zeigt Bilder für TürAuf oder TürZu
        private Image image_door_open; //Bild für TürAuf
        private Image image_door_close; //Bild für Tür Zu
        private Image image_upward; //Bild für Pfeil nach oben
        private Image image_downward; //Bild für Pfeil nach unten
        private Button[] button_intern; //interne Fahrwunschbuttons
        private Button[] button_upward; //äußere Buttons für Fahrwunsch nach oben
        private Button[] button_downward; //äußere Buttons für Fahrwunsch nach unten

        private List<bool> intern_requireds;
        private List<bool> downwards_requireds;
        private List<bool> upwards_requireds;
        private int floor;
        private int passengers;
        private bool busy;
        private Defaults.Direction current_direction;

        #endregion

        #region Konstruktoren

        /// <summary>
        /// Konstruktor
        /// </summary>
        public UserInterface()
        {
            InitializeComponent(); //Intialisierung der statisch erzeugten Formularelemente


            GroupBox_floors = new GroupBox[Defaults.Floors];
            label_floor_numbers = new Label[Defaults.Floors];
            label_current_position = new Label[Defaults.Floors];
            PictureBox_doorstates = new PictureBox[Defaults.Floors];
            button_intern = new Button[Defaults.Floors];
            button_upward = new Button[Defaults.Floors];
            button_downward = new Button[Defaults.Floors];

            image_upward = Image.FromFile(Defaults.GetProjectPath() + @"\Pictures\Aufwaerts.gif");
            image_downward = Image.FromFile(Defaults.GetProjectPath() + @"\Pictures\Aufwaerts.gif");
            image_downward.RotateFlip(RotateFlipType.Rotate180FlipX);
            pictureBox_direction.Image = image_upward;
            image_door_open = Image.FromFile(Defaults.GetProjectPath() + @"\Pictures\Aufzugtueren_auf.gif");
            image_door_close = Image.FromFile(Defaults.GetProjectPath() + @"\Pictures\Aufzugtueren_zu.gif");
            current_direction = Defaults.Direction.Upward;
            floor = 0;
            passengers = 0;

            busy = false;
            intern_requireds = new List<bool>();
            upwards_requireds = new List<bool>();
            downwards_requireds = new List<bool>();

            pictureBox_direction.Image = image_upward;

            for (int i = Defaults.Floors - 1; i >= 0; i--)
            {
                GroupBox_floors[i] = new GroupBox();
                GroupBox_floors[i].Text = "";
                GroupBox_floors[i].Location = new Point(30, Defaults.Floors * 100 - 110 * i);
                GroupBox_floors[i].Height = 110;
                GroupBox_floors[i].Width = 350;
                GroupBox_floors[i].Name = "groupBox_floor" + i;
                groupBox_outsite.Controls.Add(GroupBox_floors[i]);


                label_floor_numbers[i] = new Label();
                switch (i)
                {
                    case 0: label_floor_numbers[i].Text = "UG"; break;
                    case 1: label_floor_numbers[i].Text = "EG"; break;
                    default: label_floor_numbers[i].Text = (i - 1) + ". OG"; break;
                }
                label_floor_numbers[i].Width = 70;
                label_floor_numbers[i].Location = new Point(10, 10);
                label_floor_numbers[i].Name = "label_floor" + i;
                GroupBox_floors[i].Controls.Add(label_floor_numbers[i]);
                label_floor_numbers[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                PictureBox_doorstates[i] = new PictureBox();
                PictureBox_doorstates[i].Width = 30;
                PictureBox_doorstates[i].Height = 30;
                PictureBox_doorstates[i].Location = new Point(100, 50);
                PictureBox_doorstates[i].Name = "pictureBox_doorstate" + i;
                PictureBox_doorstates[i].Image = image_door_close;
                GroupBox_floors[i].Controls.Add(PictureBox_doorstates[i]);

                label_current_position[i] = new Label();
                label_current_position[i].Text = "#Position";
                label_current_position[i].Name = "label_position" + i;
                label_current_position[i].Width = 60;
                label_current_position[i].Location = new Point(100, 12);
                GroupBox_floors[i].Controls.Add(label_current_position[i]);


                button_intern[i] = new Button();
                button_intern[i].Location = new Point(20, Defaults.Floors * 45 - 45 * i);
                button_intern[i].Height = 40;
                button_intern[i].Width = 100;
                button_intern[i].Name = "Button_int" + i;
                switch (i)
                {
                    case 0: button_intern[i].Text = "1. UG"; break;
                    case 1: button_intern[i].Text = "EG"; break;
                    default: button_intern[i].Text = (i - 1) + ". OG"; break;
                }
                groupBox_floor_selection.Controls.Add(button_intern[i]);
                button_intern[i].Click += new System.EventHandler(ClickInnerButton);

                if (i != Defaults.Floors - 1)
                {
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
                    button_downward[i].Name = "Button_down_" + i;
                    GroupBox_floors[i].Controls.Add(button_downward[i]);
                    button_downward[i].Click += new System.EventHandler(ClickOutsideButton);

                }

                intern_requireds.Add(false);
                upwards_requireds.Add(false);
                downwards_requireds.Add(false);
            }// Ende der for-Schleife       
            CurrentPosition = floor;
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
            {
                List<bool> _upwards = value;
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
                for (int i = 0; i < Defaults.Floors; i++)
                {
                    if (i == 0)
                    {
                        _downwards.Add(false);
                    }
                    else
                    {
                        if (button_downward[i].Enabled == false) _downwards.Add(true);
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
                for (int i = 0; i < Defaults.Floors; i++)
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
                if (current_direction == Defaults.Direction.Upward) pictureBox_direction.Image = image_upward;
                else pictureBox_direction.Image = image_downward;

                if (pos > Defaults.Floors || pos < 0) return;
                switch (pos)
                {
                    case 0: position_value = "UG"; break;
                    case 1: position_value = "EG"; break;
                    default: position_value = pos - 1 + ". OG"; break;
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
                        GroupBox_floors[i].BackColor = Color.WhiteSmoke;
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
            PictureBox_doorstates[Defaults.FloorToIdx(floor)].Image = image_door_open;

        }

        /// <summary>
        /// Tür schießen auf der GUI darstellen
        /// </summary>
        /// <param name="floor"></param>
        public void gui_close_door()
        {
           
            PictureBox_doorstates[Defaults.FloorToIdx(floor)].Image = image_door_close;

        }

        /// <summary>
        /// Funktionilitäten beim Öffnen der Tür
        /// </summary>
        public void openDoor()
        {
            button_less_passenger.Enabled = true;
            button_more_passenger.Enabled = true;

            gui_open_door();
            DeleteReqired();

            timer_tuer_zu.Stop();
            timer_tuer_zu.Start();
        }

        /// <summary>
        /// Funktionilitäten beim Schließen der Tür
        /// </summary>

        public void closeDoor()
        {
            button_less_passenger.Enabled = false;
            button_more_passenger.Enabled = false;
            gui_close_door();


            timer_tuer_zu.Stop();
        }

        /// <summary>
        /// Abarbeitung des Fahrstuhlalgorithmzus
        /// </summary>
        public void go()
        {

            if (checkforoverload() == true)
            {
                return;

            }
            else if (wishesHere() == true)
            {
                openDoor();
                button_open_door.Enabled = true;
                button_close_door.Enabled = true;
                busy = true;
                //return;
            }

            else if (wishesInDirection() == true)
            {
                floorchange();
                busy = true;
                timer_fahren.Start();
                // return;
            }


            else if (wishesHereInOppositeDirection())
            {
                switchDirection();
                busy = true;
                button_open_door.Enabled = true;
                button_close_door.Enabled = true;
                openDoor();
                //return;
            }


            else if (WishesInOppositeDirection())
            {
                switchDirection();
                busy = true;
                floorchange();
                timer_fahren.Start();
                //return;
            }

            else busy = false;
        }

        /// <summary>
        /// Behandlung von Etagenwechsel
        /// </summary>
        public void floorchange()
        {
            switch (current_direction)
            {
                case Defaults.Direction.Upward: floor++; break;
                case Defaults.Direction.Downward: floor--; break;
            }

            button_close_door.Enabled = false;
            button_open_door.Enabled = false;

            if ((floor == -Defaults.Basements && current_direction == Defaults.Direction.Downward) ||
                (floor == (Defaults.Floors - Defaults.Basements - 1) && current_direction == Defaults.Direction.Upward)) //ganz oben oder ganz unten --> Richtungswechsel
            {
                switchDirection();
            }

            pictureBox_direction.Visible = true;


        }

        /// <summary>
        /// Richtungswechsel
        /// </summary>
        public void switchDirection()
        {
            switch (current_direction)
            {
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
                        if (upwards_requireds[Defaults.FloorToIdx(floor)] == true)
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

        /// <summary
        /// Gibt auf einer Etage in Fahrtrichtung Wünsche von innen oder von außen
        /// </summary>
        /// <returns>true or false</returns>

        public bool wishesInDirection()
        {
            switch (current_direction)
            {
                case Defaults.Direction.Upward:
                    {
                        for (int IDX = Defaults.FloorToIdx(floor); IDX < Defaults.Floors; IDX++)
                        {
                            if (upwards_requireds[IDX] == true || intern_requireds[IDX] == true || downwards_requireds[IDX] == true)
                                return true;
                        }
                    } break;

                case Defaults.Direction.Downward:
                    {
                        for (int IDX = 0; IDX < Defaults.FloorToIdx(floor); IDX++)
                        {
                            if (intern_requireds[IDX] == true || downwards_requireds[IDX] == true || upwards_requireds[IDX] == true)
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

        public bool WishesInOppositeDirection()
        {
            switch (current_direction)
            {
                case Defaults.Direction.Upward:
                    {
                        for (int IDX = 0; IDX < Defaults.FloorToIdx(floor); IDX++)
                        {
                            if (downwards_requireds[IDX] == true || intern_requireds[IDX] == true || upwards_requireds[IDX] == true)
                                return true;
                        }
                    } break;

                case Defaults.Direction.Downward:
                    {
                        for (int IDX = Defaults.Floors - Defaults.Basements; IDX > Defaults.FloorToIdx(floor); IDX--)
                        {
                            if (upwards_requireds[IDX] == true || intern_requireds[IDX] == true || downwards_requireds[IDX] == true)
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

            if (floor < Defaults.Floors - Defaults.Basements - 1)
            {
                upwards_requireds[Defaults.FloorToIdx(floor)] = false;
                button_upward[Defaults.FloorToIdx(floor)].Enabled = true;
            }

            if (floor > (-Defaults.Basements))
            {
                downwards_requireds[Defaults.FloorToIdx(floor)] = false;
                button_downward[Defaults.FloorToIdx(floor)].Enabled = true;

            }

            intern_requireds[Defaults.FloorToIdx(floor)] = false;
            button_intern[Defaults.FloorToIdx(floor)].Enabled = true;
        }

        /// <summary>
        /// Überprüft ob Überladen
        /// </summary>
        /// <returns></returns>
        public bool checkforoverload()
        {
            if (passengers > Defaults.MaximumPassengers) return true;
            return false;

        }
        #endregion


        #region Button Clicks

        /// <summary>
        /// Der +1 Button wurde gedrückt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_more_passenger_Click(object sender, EventArgs e)
        {

            PassengersCount = ++passengers;

            if (passengers > 0)
            {
                button_less_passenger.Enabled = true;
            }
        }
        /// <summary>
        /// Der -1 Button wurde gedrückt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_less_passenger_Click(object sender, EventArgs e) //-1 Button
        {
            if (passengers > 0)
            {
                PassengersCount = --passengers;
            }
            if (passengers == 0)
            {
                button_less_passenger.Enabled = false;
            }
        }

        /// <summary>
        /// Notfallbutton wurde gedrückt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_emergency_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Notruf betätigt. Verbindung zur Zentrale wird hergestellt.");
        }

        /// <summary>
        /// Türöffnungsbutton wurde gedrückt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_open_door_Click(object sender, EventArgs e)
        {

            openDoor();
        }
        /// <summary>
        /// Türschließungsbutton wurde gedrückt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        /// <summary>
        /// Innerer Wunsch wurde betätigt
        /// </summary>
        /// <param name="sender">Der jeweilige Button</param>
        /// <param name="e"></param>
        private void ClickInnerButton(object sender, EventArgs e)
        {
            Button currentButton = sender as Button;
            currentButton.Enabled = false;
            intern_requireds = InternRequired;
            if (!busy)
            {
                timer_tuer_zu.Start();
            }
        }

        /// <summary>
        /// Äußere Wunsch wurde betätigt
        /// </summary>
        /// <param name="sender">Der jeweilige Button wurde betätigt</param>
        /// <param name="e"></param>
        private void ClickOutsideButton(object sender, EventArgs e)
        {
            Button currentButt = sender as Button;
            currentButt.Enabled = false;
            upwards_requireds = UpwardRequired;
            downwards_requireds = DownwardRequired;
            if (!busy)
            {
                timer_tuer_zu.Start();
            }

        }
        #endregion


        /// <summary>
        /// Türschließungstimer abgelaufen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_tuer_zu_Tick(object sender, EventArgs e)
        {
            timer_tuer_zu.Stop();
            if (checkforoverload() == true)
            {
                timer_tuer_zu.Start();
            }
            else
            {
                closeDoor();
                go();
            }


        }


        /// <summary>
        /// Fahr-Timer abgelaufen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_fahren_Tick(object sender, EventArgs e)
        {
            timer_fahren.Stop();
            pictureBox_direction.Visible = false;
            CurrentPosition = floor;
            go();


        }

    }

}
