using System;
using System.Drawing;
using System.Windows.Forms;

namespace PR_3
{
    /* 
     * Описание программы:
     * 
     * Реализован однооконный интерфейс с тремя разделами
     *  - Главное меню
     *  Содержит поле приветствия, после ввода имени, кнопки перехода в разделы "Таймер" и "Игра".
     *  Поле приветствия выводит фразу по умолчанию, если в поле ввода имени ничего не введено.
     *  Иначе предлагает пользователю выбрать интересующий его раздел.
     *  
     *  - Таймер
     *  Содержит поле времени таймера, кнопки управления таймером, кнопку перехода в главное меню.
     *  При нажатии кнопки "Запустить таймер" происходит изменение показаний в поле времени таймера с заданным интервалом.
     *  При нажатии кнопки "Остановить таймер" таймер встает на паузу.
     *  При нажатии кнопки "Назад" все текущие элементы на форме удаляются, появляются элементы главного меню.
     *  При возвращении в данный раздел таймер обнуляется (создается новый экземпляр класса).
     *  
     *  - Игра
     *  Содержит игровую кнопку, которую нужно поймать и нажать, и кнопку перехода в главное меню.
     *  При наведении мыши на игровую кнопку ее положение меняется (моментально).
     *  Исключена возможность нажатия кнопки посредством клавиши Tab (свойство TabStop).
     *  Несмотря на это, написана ветка на случай нажатия игровой кнопки (выводится сообщение о победе)
     * 
     */

    public partial class TitleForm : Form
    {
        Label lblName;
        Label lblTime;
        TextBox tbName;
        Button[] btn;
        Font cmnFont;
        Size cmnSize;
        DateTime dt;
        Timer timer;

        public TitleForm()
        {
            InitializeComponent();
            startWindow();
        }

        private void startWindow()
        {
            this.Text = "Главное меню";
            this.Size = new Size(400, 350);
            cmnSize = new Size(350, 50);

            cmnFont = new Font("Arial", 12);

            lblName = new Label();
            lblName.Text = "Введите ваше имя";
            lblName.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblName);

            tbName = new TextBox();
            tbName.TextAlign = HorizontalAlignment.Center;
            tbName.TextChanged += tbxTextChanged;
            this.Controls.Add(tbName);

            String[] btnText = { "Таймер", "Игра"};

            btn = new Button[2];
            for (int i = 0; i < btn.Length; i++)
            {
                btn[i] = new Button();
                btn[i].TextAlign = ContentAlignment.MiddleCenter;
                btn[i].Text = btnText[i];
                btn[i].Click += btnClick;
                this.Controls.Add(btn[i]);
            }

            pasteElementsByGrid();
        }

        private void tbxTextChanged(object sender, EventArgs e)
        {
            if (tbName.Text == "")
                lblName.Text = "Введите ваше имя";
            else
                lblName.Text = $"Здравствуйте, {tbName.Text}! Выберите, что хотите сделать";

        }

        private void pasteElementsByGrid()
        {
            // расставляет все элементы на поле по сетке (используется для главной формы и формы таймера)
            int locCounter = 0;
            foreach (Control ctr in this.Controls)
            {
                ctr.Size = cmnSize;
                ctr.Font = cmnFont;
                ctr.Location = new Point(20, 20 + 70 * locCounter++);
            }
        }

        private void btnClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Text)
            {
                case "Таймер":
                    this.Controls.Clear();
                    timerWindow();
                    break;
                case "Игра":
                    this.Controls.Clear();
                    gameWindow();
                    break;
                case "Назад":
                    this.Controls.Clear();
                    startWindow();
                    break;
                case "Запустить таймер":
                    timer.Start();
                    break;
                case "Остановить таймер":
                    timer.Stop();
                    break;
                case "Нажми меня":
                    MessageBox.Show("Ты выиграл"); // если успеешь:)
                    break;
                default:
                    break;
            }
        }

        private void timerWindow()
        {
            this.Text = "Таймер";
            addTimer();
            addBackButton();
        }

        private void gameWindow()
        {
            this.Text = "Игра";
            addRandomButton();
            addBackButton();
        }

        private void addBackButton()
        {
            Button btn = new Button();
            btn.Font = cmnFont;
            btn.Text = "Назад";
            btn.Location = new Point(20, 230);
            btn.Size = cmnSize;
            btn.Click += btnClick;
            this.Controls.Add(btn);
        }

        private void timeTickEvent(object sender, EventArgs e)
        {
            dt = dt + new TimeSpan(0, 0, 1);
            lblTime.Text = dt.ToString("HH:mm:ss");
        }

        private void addTimer()
        {
            dt = new DateTime();
            dt = dt.Date + new TimeSpan(0, 0, 0);
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += timeTickEvent;

            lblTime = new Label();
            lblTime.Text = "00:00:00";
            lblTime.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTime);

            lblTime.Text = dt.ToString("HH:mm:ss");

            String[] btnText = { "Запустить таймер", "Остановить таймер" };
            Button[] btnTimer = new Button[2];
            for (int i = 0; i < btnTimer.Length; i++)
            {
                btnTimer[i] = new Button();
                btnTimer[i].TextAlign = ContentAlignment.MiddleCenter;
                btnTimer[i].Text = btnText[i];
                btnTimer[i].Click += btnClick;
                this.Controls.Add(btnTimer[i]);
            }
            pasteElementsByGrid();
        }

        private void addRandomButton()
        {
            Button btnPushMe = new Button();
            btnPushMe.Location = new Point(150, 100);
            btnPushMe.Text = "Нажми меня";
            btnPushMe.Size = new Size(80, 50);
            btnPushMe.Font = cmnFont;
            btnPushMe.TabStop = false; // чтоб нельзя было выиграть:)) 
            btnPushMe.MouseMove += btnMouseMove;
            this.Controls.Add(btnPushMe);
        }

        private void btnMouseMove(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            Random r = new Random();
            btn.Left = r.Next(0, this.Size.Width - btn.Width);
            btn.Top = r.Next(00, 170);
        }

    }
}
