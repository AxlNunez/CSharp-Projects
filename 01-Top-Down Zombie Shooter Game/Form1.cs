namespace Shooter
{
    public partial class Form1 : Form
    {
        bool goLeft, goRight, goUp, goDown, gameOver;
        string facing = "up";
        int playerHealth = 100;
        int speed = 10;
        int ammo = 15;
        int zombieSpeed = 3;
        Random randNum = new Random();
        int kills;
        List<PictureBox> zombiesList = new List<PictureBox>();

        public Form1()
        {
            InitializeComponent();
            RestartGame();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            if (playerHealth > 1)
            {
                healthBar.Value = playerHealth;
            }
            else 
            {
                gameOver = true;
                player.Image = Properties.Resources.dead;
                gameTimer.Stop();
            }

            ammoCount.Text = "Ammo: " + ammo;
            killCount.Text = "Kills: " + kills;

            if (goLeft == true && player.Left > 0)
            {
                player.Left -= speed;
            }
            if (goRight == true && player.Left + player.Width< this.ClientSize.Width)
            {
                player.Left += speed;
            }
            if (goUp == true && player.Top > 40)
            { player.Top -= speed;}
            if (goDown == true && player.Top + player.Height< this.ClientSize.Height)
            {
                player.Top += speed;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ammo")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        ammo += 5;
                    }
                }


                if (x is PictureBox && (string)x.Tag == "zombie")
                {

                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        playerHealth -= 1;
                    }


                    if(x.Left > player.Left)
                    {
                        x.Left -= zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.z_left;
                    }

                    if (x.Left < player.Left)
                    {
                        x.Left += zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.z_right;
                    }

                    if (x.Top > player.Top)
                    {
                        x.Top -= zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.z_up;
                    }

                    if (x.Top < player.Top)
                    {
                        x.Top += zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.z_down;
                    }
                   
                }

                foreach (Control j in this.Controls)
                {
                    if (j is PictureBox &&(string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "zombie")
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            kills++;

                            this.Controls.Remove(j);
                            ((PictureBox)j).Dispose();
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            zombiesList.Remove(((PictureBox)x));
                            MakeZombies();
                        }
                    }

                }


            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)

        {
            if (gameOver == true)
            { return;
            }

            if(e.KeyCode == Keys.Left)
            { 
                goLeft = true;
                facing = "left";
                player.Image = Properties.Resources.left;
            }

            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
                facing = "right";
                player.Image = Properties.Resources.right;
            }

            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
                facing = "up";
                player.Image = Properties.Resources.up;
            }

            if (e.KeyCode == Keys.Down)
            {
                goDown = true;
                facing = "down";
                player.Image = Properties.Resources.down;
            }

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }

            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }

            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }

            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }

            if (e.KeyCode == Keys.Space && ammo > 0 && gameOver == false)
            {
                ammo--;
                ShootBullet(facing);

                if (ammo < 1)
                {
                    DropAmmo();
                }
            }

            if (e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }
        }

        private void ShootBullet(string direction)
        {
            Bullet shootBullet = new Bullet();
            shootBullet.direction = direction;
            shootBullet.bulletLeft = player.Left + (player.Width / 2);
            shootBullet.bulletTop = player.Top +(player.Height / 2);
            shootBullet.MakeBullet(this);
        }

        private void MakeZombies()
        {
            PictureBox zombie = new PictureBox();
            zombie.Tag = "zombie";
            zombie.Image = Properties.Resources.z_down;
            zombie.Left = randNum.Next(0, 900);
            zombie.Top = randNum.Next(0, 800);
            zombie.SizeMode = PictureBoxSizeMode.AutoSize;
            zombiesList.Add(zombie);
            this.Controls.Add(zombie);
            player.BringToFront();
        }

        private void DropAmmo()
        {

            PictureBox ammo = new PictureBox();
            ammo.Image = Properties.Resources.ammo_Image;
            ammo.SizeMode = PictureBoxSizeMode.AutoSize;
            ammo.Left = randNum.Next(10, this.ClientSize.Width - ammo.Width);
            ammo.Top = randNum.Next(60, this.ClientSize.Height - ammo.Height);
            ammo.Tag = "ammo";
            this.Controls.Add(ammo);

            ammo.BringToFront();
            player.BringToFront();
        }

        private void RestartGame()
        {
            player.Image = Properties.Resources.up;

            foreach (PictureBox i in zombiesList)
            {
                this.Controls.Remove(i);
            }

            zombiesList.Clear();

            for (int i = 0; i < 3; i++)
            {
                MakeZombies();
            }

            goUp = false;
            goDown = false;
            goLeft = false;
            goRight = false;
            gameOver = false;

            playerHealth = 100;
            kills = 0;
            ammo = 15;


            gameTimer.Start();
   

        }
    }
}