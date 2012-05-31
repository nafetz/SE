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

        private Label[] floor_numbers; //Anzeige der Etatennummern
        private Label[] current_position; //Anzeige der aktuellen Etage (gleich in allen Labels)
        private GroupBox[] floors;    //GroupBoxen für die Etagen
        private CheckedListBox[] required; //Fahrwünsche in den jeweiligen Etagen
        private PictureBox[] doorstates;
        private Image door1;
        private Image door2;

        #endregion

        #region Konstruktoren

        /// <summary>
        /// Konstruktor
        /// </summary>
        public UserInterface()
        {
            InitializeComponent(); //Intialisierung der statisch erzeugten Formularelemente

            _passengersIO = Defaults.MoreOrLess.Neither;

            floors = new GroupBox[Defaults.Floors];
            floor_numbers = new Label[Defaults.Floors];
            current_position = new Label[Defaults.Floors];
            required = new CheckedListBox[Defaults.Floors];
            doorstates = new PictureBox[Defaults.Floors];

            door1 = Image.FromFile(Defaults.GetProjectPath() + @"\Pictures\Aufzugtueren_auf.gif");
            door2 = Image.FromFile(Defaults.GetProjectPath() + @"\Pictures\Aufzugtueren_zu.gif");


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
                doorstates[i].Image = door1;
                floors[i].Controls.Add(doorstates[i]);
                
                current_position[i] = new Label();
                current_position[i].Text = "#Position";
                current_position[i].Name = "label_position" + i;
                current_position[i].Width = 60;
                current_position[i].Location = new Point(100, 12);
                floors[i].Controls.Add(current_position[i]);
                

                required[i] = new CheckedListBox();
                required[i].Location = new Point(210, 20);
                required[i].Height = 50;
                required[i].Name = "checkedListBox_floor" + i;
                required[i].BackColor = System.Drawing.SystemColors.Control;
                if(i!=Defaults.Floors - 1 ) required[i].Items.Add("Aufwärts");
                if(i!=0)               required[i].Items.Add("Abwärts");
                required[i].BorderStyle = System.Windows.Forms.BorderStyle.None;
                required[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                floors[i].Controls.Add(required[i]);                   
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
                        if (required[i].GetItemCheckState(0) == CheckState.Checked) _upwards.Add(true);
                        else _upwards.Add(false);
                    }
                }
                return _upwards;
            }// get

            set
            {       List<bool> _upwards = value; //kann man die Liste einfach so kopieren?
                    for (int i = 0; i < Defaults.Floors - 1; i++) //geht bis zum vorletzten, da es oben ohnehin kein "hoch" gibt
                    {
                        if (_upwards.ElementAt(i) == true) required[i].SetItemChecked(0, false);
                    
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
                        if( required[ i ].GetItemChecked( 1 ) ) _downwards.Add( true );
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
                    if (_downwards.ElementAt(i) == true) required[i].SetItemChecked(0, false);

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
                for (int i = checkedListBox_floor_selection.Items.Count; i > 0; i--)
                {
                    if (checkedListBox_floor_selection.GetItemChecked(i)) _interns.Add(true);
                    else _interns.Add(false);
                }
                return _interns;
            }
            set
            {
                List<bool> _interns = value; //kann man die Liste einfach so kopieren?
                for (int i = checkedListBox_floor_selection.Items.Count; i > 0; i--) //startet bei 1, da es unten ohnehin kein "runter" gibt
                {
                    if (_interns.ElementAt(i) == true) checkedListBox_floor_selection.SetItemChecked(0, false);

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
                int pos = value;
                String position_value;
                if (pos > Defaults.Floors || pos < 0) return;
                switch (pos)
                {
                    case 0: position_value = "1. UG"; break;
                    case 1: position_value = "EG"; break;
                    default: position_value = pos + ". OG"; break;
                }

                foreach (Label poslabel in current_position)
                {
                    poslabel.Text = position_value; //äußere Label
                }

                label_floor_display.Text = position_value; //Label im inneren
            }
        }

        /// <summary>
        /// Wert zur Anzeige der aktuellen Richtung
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
                            // dein Code hier
                        } break;
                    case Defaults.Direction.Upward:
                        {
                            // und hier
                        } break;
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

                

            }
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
            if (doorstates[floor].Image == door2) doorstates[floor].Image = door1;
        }

        /// <summary>
        /// Tür schießen auf der GUI darstellen
        /// </summary>
        /// <param name="floor"></param>
        public void close_door(int floor)
        {
            if (floor < 0 || floor > Defaults.Floors) return;
            if (doorstates[floor].Image == door1) doorstates[floor].Image = door2;

        }

        public void ResetPassengerIO() 
        {
            _passengersIO = Defaults.MoreOrLess.Neither;
        }

        #endregion

        private void button_more_passenger_Click(object sender, EventArgs e) //+1 Button
        {
            _passengersIO = Defaults.MoreOrLess.More; //Wo ist der Typ von _passengersIO deklariert? wirkt unsauber implementiert?!
            Defaults.ManualResetEvent.Set();
        }

        private void button_less_passenger_Click(object sender, EventArgs e) //-1 Button
        {
            _passengersIO = Defaults.MoreOrLess.Less; 
            Defaults.ManualResetEvent.Set();
        }

        private void button_emergency_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Blöd gelaufen... äh gefahren");  
        }

        private void button_open_door_Click(object sender, EventArgs e)
        {
            open_door(1); // Hier müssen wir die aktuelle Etage ermitteln
        }

        private void button_close_door_Click(object sender, EventArgs e)
        {
            close_door(1); // Hier müssen wir die aktuelle Etage ermitteln
        }

    }
}
