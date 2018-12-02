using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiamondInTheWater;
using DiamondInTheWater.Entities;
using DiamondInTheWater.Screens;
using DiamondInTheWater.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DiamondInTheWater.Screens
{
    public class GameScreen : Screen
    {
        private enum UserInterfaceForms
        {
            END_DAY,

            END_GAME,

            GAMEWORLD,
            
            PRODUCTION,

            TRADE,

            LOGISTICS,
        }

        private enum GameTransitionState
        {
            FADE_IN,
            DO_STUFF,
            FADE_OUT,
        }

        private GameTransitionState state;
        private UserInterfaceForms form;
        private bool isUiActive;
        private SpriteFont font, fontb;
        private Game1 game;
        private GameWorld world;
        private Map waterMap;
        private Texture2D blank, pelkFrown, pelkSmile, mute, notMute, halfMute;
        private ContentManager Content;
        private List<float> vals;
        private UIButton[] gameworldBtns, productionBtns, tradeButtons;
        private MusicManager manager;
        private Texture2D[] goodIcons, apIcons;
        private float opacity;
        private bool isTransitioning, isYourInventory;
        private const float FADE_INTERVAL = 0.05f;
        private int selectedNation;
        private float chocAdd, phoneAdd, shirtAdd, chocAdd2, phoneAdd2, shirtAdd2, timer;
        private bool isGoodTrade;

        /// <summary>
        /// Creates a new instance of the <c>GameScreen</c>.
        /// </summary>
        public GameScreen(Game1 game)
        {
            isGoodTrade = false;
            selectedNation = 1;
            state = GameTransitionState.FADE_IN;
            isTransitioning = false;
            this.game = game;
            world = new GameWorld(game);
            isUiActive = true;
            isYourInventory = true;
            form = UserInterfaceForms.GAMEWORLD;
            manager = new MusicManager();
            MediaPlayer.Volume = 1.0f;
        }

        public void LoadGame()
        {
            WorldSave save = (WorldSave)GameSerializer.Deserialize("Save/Load.sav");
            world.LoadGame(save);
        }

        /// <summary>
        /// Initializes the <c>GameScreen</c>.
        /// </summary>
        /// <param name="Content"></param>
        public override void Initialize(ContentManager Content)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            goodIcons = new Texture2D[6];
            apIcons = new Texture2D[5];
            goodIcons[0] = Content.Load<Texture2D>("FactoryIcon");
            goodIcons[1] = Content.Load<Texture2D>("TruckIcon");
            goodIcons[2] = Content.Load<Texture2D>("ToolIcon");
            goodIcons[3] = Content.Load<Texture2D>("ChocolateIcon");
            goodIcons[4] = Content.Load<Texture2D>("ShirtIcon");
            goodIcons[5] = Content.Load<Texture2D>("PhoneIcon");
            pelkFrown = Content.Load<Texture2D>("PelkeyFrown");
            pelkSmile = Content.Load<Texture2D>("PelkeySmile");
            mute = Content.Load<Texture2D>("Muted");
            notMute = Content.Load<Texture2D>("NonMuted");
            halfMute = Content.Load<Texture2D>("HalfMuted");
            manager.Load(Content);

            for (int i = 1; i <= 5; i++)
            {
                apIcons[i - 1] = Content.Load<Texture2D>("AP" + i);
            }

            font = this.Content.Load<SpriteFont>("defaultFont");
            blank = this.Content.Load<Texture2D>("blank");
            world.Initialize(this.Content);

            int size = Map.TILE_SCALE * 32;
            waterMap = new Map(game.Width / size + 1, game.Height / size + 1);

            fontb = this.Content.Load<SpriteFont>("largeFont");
            int space = (game.Width - 4 * 200) / 5;

            #region Game world form elements
            gameworldBtns = new UIButton[5];
            gameworldBtns[0] = new UIButton();
            gameworldBtns[1] = new UIButton();
            gameworldBtns[2] = new UIButton();
            gameworldBtns[3] = new UIButton();
            gameworldBtns[4] = new UIButton();

            gameworldBtns[0].Texture = blank;
            gameworldBtns[0].Background = Color.CornflowerBlue;
            gameworldBtns[0].Size = new Point(200, 70);
            gameworldBtns[0].Position = new Point(space, game.Height - 100);
            gameworldBtns[0].Font = fontb;
            gameworldBtns[0].Text = "Next Day";
            gameworldBtns[0].OnClick += OnNextDayClick;
            gameworldBtns[1].Texture = blank;
            gameworldBtns[1].Background = Color.CornflowerBlue;
            gameworldBtns[1].Size = new Point(200, 70);
            gameworldBtns[1].Position = new Point(space * 2 + 200, game.Height - 100);
            gameworldBtns[1].Font = fontb;
            gameworldBtns[1].Text = "Produce";
            gameworldBtns[1].OnClick += OnProductionClick;
            gameworldBtns[2].Texture = blank;
            gameworldBtns[2].Background = Color.CornflowerBlue;
            gameworldBtns[2].Size = new Point(200, 70);
            gameworldBtns[2].Position = new Point(space * 3 + 400, game.Height - 100);
            gameworldBtns[2].Font = fontb;
            gameworldBtns[2].OnClick += OnTradeClick;
            gameworldBtns[2].Text = "Trade";
            gameworldBtns[3].Texture = blank;
            gameworldBtns[3].Background = Color.CornflowerBlue;
            gameworldBtns[3].Size = new Point(200, 70);
            gameworldBtns[3].Position = new Point(space * 4 + 600, game.Height - 100);
            gameworldBtns[3].Font = fontb;
            gameworldBtns[3].OnClick += OnLogisticsClick;
            gameworldBtns[3].Text = "Advisor";
            gameworldBtns[4].Texture = notMute;
            gameworldBtns[4].Background = Color.CornflowerBlue;
            gameworldBtns[4].Size = new Point(22 * 3, 21 * 3);
            gameworldBtns[4].Position = new Point(game.Width - 76, game.Height - 73);
            gameworldBtns[4].Font = fontb;
            gameworldBtns[4].Text = "";
            gameworldBtns[4].OnClick += onToggleMute;
            #endregion

            #region Production form elements
            Texture2D upBtn = Content.Load<Texture2D>("Up");
            Texture2D downBtn = Content.Load<Texture2D>("Down");
            productionBtns = new UIButton[12];
            productionBtns[0] = new UIButton();
            productionBtns[0].Texture = upBtn;
            productionBtns[0].OnClick += OnProductionBtnClick;
            productionBtns[1] = new UIButton();
            productionBtns[1].Texture = upBtn;
            productionBtns[1].OnClick += OnProductionBtnClick;
            productionBtns[2] = new UIButton();
            productionBtns[2].Texture = upBtn;
            productionBtns[2].OnClick += OnProductionBtnClick;
            productionBtns[3] = new UIButton();
            productionBtns[3].Texture = upBtn;
            productionBtns[3].OnClick += OnProductionBtnClick;
            productionBtns[4] = new UIButton();
            productionBtns[4].Texture = upBtn;
            productionBtns[4].OnClick += OnProductionBtnClick;
            productionBtns[5] = new UIButton();
            productionBtns[5].Texture = upBtn;
            productionBtns[5].OnClick += OnProductionBtnClick;
            productionBtns[6] = new UIButton();
            productionBtns[6].Texture = downBtn;
            productionBtns[6].OnClick += OnProductionBtnClick;
            productionBtns[7] = new UIButton();
            productionBtns[7].Texture = downBtn;
            productionBtns[7].OnClick += OnProductionBtnClick;
            productionBtns[8] = new UIButton();
            productionBtns[8].Texture = downBtn;
            productionBtns[8].OnClick += OnProductionBtnClick;
            productionBtns[9] = new UIButton();
            productionBtns[9].Texture = downBtn;
            productionBtns[9].OnClick += OnProductionBtnClick;
            productionBtns[10] = new UIButton();
            productionBtns[10].Texture = downBtn;
            productionBtns[10].OnClick += OnProductionBtnClick;
            productionBtns[11] = new UIButton();
            productionBtns[11].Texture = downBtn;
            productionBtns[11].OnClick += OnProductionBtnClick;
            #endregion

            #region Trade form elements
            tradeButtons = new UIButton[8];

            tradeButtons[0] = new UIButton();
            tradeButtons[0].OnClick += selectNation1;
            tradeButtons[0].Texture = Content.Load<Texture2D>("Berkeley");
            tradeButtons[0].Size = new Point(64, 64);

            tradeButtons[1] = new UIButton();
            tradeButtons[1].OnClick += selectNation2;
            tradeButtons[1].Texture = Content.Load<Texture2D>("Sun");
            tradeButtons[1].Size = new Point(64, 64);

            tradeButtons[2] = new UIButton();
            tradeButtons[2].OnClick += changeYourInventory;
            tradeButtons[2].Texture = Content.Load<Texture2D>("Blank");
            tradeButtons[2].Size = new Point(222, 64);
            tradeButtons[2].Text = "Your Inventory";
            tradeButtons[2].Background = Color.LightGreen;
            tradeButtons[2].Font = fontb;

            tradeButtons[3] = new UIButton();
            tradeButtons[3].OnClick += changeTheirInventory;
            tradeButtons[3].Texture = Content.Load<Texture2D>("Blank");
            tradeButtons[3].Background = Color.Green;
            tradeButtons[3].Size = new Point(222, 64);
            tradeButtons[3].Text = "Their Inventory";
            tradeButtons[3].Font = fontb;

            tradeButtons[4] = new UIButton();
            tradeButtons[4].OnClick += addItemToTrade;
            tradeButtons[4].OnRightClick += removeItemFromTrade;
            tradeButtons[4].Texture = Content.Load<Texture2D>("ChocolateIcon");
        
            tradeButtons[5] = new UIButton();
            tradeButtons[5].OnClick += addItemToTrade;
            tradeButtons[5].OnRightClick += removeItemFromTrade;
            tradeButtons[5].Texture = Content.Load<Texture2D>("PhoneIcon");

            tradeButtons[6] = new UIButton();
            tradeButtons[6].OnClick += addItemToTrade;
            tradeButtons[6].OnRightClick += removeItemFromTrade;
            tradeButtons[6].Texture = Content.Load<Texture2D>("ShirtIcon");

            tradeButtons[7] = new UIButton();
            tradeButtons[7].OnClick += makeTrade;
            tradeButtons[7].Texture = Content.Load<Texture2D>("Blank");
            tradeButtons[7].Background = Color.Green;
            tradeButtons[7].Size = new Point(148, 64);
            tradeButtons[7].Text = "Trade";
            tradeButtons[7].Font = fontb;
            #endregion
        }

        private void onToggleMute(UIEventArg arg)
        {
            Texture2D texture = null;
            if (MediaPlayer.Volume == 1.0f)
            {
                MediaPlayer.Volume = 0.5f;
                texture = halfMute;
            }
            else if (MediaPlayer.Volume == 0.5f)
            {
                MediaPlayer.Volume = 0.0f;
                texture = mute;
            }
            else
            {
                MediaPlayer.Volume = 1.0f;
                texture = notMute;
            }

            arg.Component.Texture = texture;
        }

        private void makeTrade(UIEventArg arg)
        {
            if (isGoodTrade)
            {
                Nation n = world.GetPlayer();
                Nation n1 = world.Nations[selectedNation];
                n.Chocolates += chocAdd2;
                n.Phones += phoneAdd2;
                n.Shirts += shirtAdd2;
                n.Chocolates -= chocAdd;
                n.Phones -= phoneAdd;
                n.Shirts -= shirtAdd;
                n.BoughtChocolates += chocAdd2;
                n.BoughtPhones += phoneAdd2;
                n.BoughtShirts += shirtAdd2;
                n.TradeChocolates += chocAdd;
                n.TradePhones += phoneAdd;
                n.TradeShirts += shirtAdd;
                n1.Chocolates += chocAdd;
                n1.Phones += phoneAdd;
                n1.Shirts += shirtAdd;
                n1.Chocolates -= chocAdd2;
                n1.Phones -= phoneAdd2;
                n1.Shirts -= shirtAdd2;
                n1.BoughtChocolates += chocAdd;
                n1.BoughtPhones += phoneAdd;
                n1.BoughtShirts += shirtAdd;
                n1.TradeChocolates += chocAdd2;
                n1.TradePhones += phoneAdd2;
                n1.TradeShirts += shirtAdd2;

                ResetTrade();
            }
        }

        private void addItemToTrade(UIEventArg arg)
        {
            Nation n = world.GetPlayer();
            Nation n1 = world.Nations[selectedNation];
            if (arg.Component == tradeButtons[4])
            {
                if (isYourInventory)
                    chocAdd = Math.Min(chocAdd + 1, (int)n.Chocolates);
                else
                    chocAdd2 = Math.Min(chocAdd2 + 1, (int)n1.Chocolates);
            }
            else if (arg.Component == tradeButtons[5])
            {
                if (isYourInventory)
                    phoneAdd = Math.Min(phoneAdd + 1, (int)n.Phones);
                else
                    phoneAdd2 = Math.Min(phoneAdd2 + 1, (int)n1.Phones);
            }
            else if (arg.Component == tradeButtons[6])
            {
                if (isYourInventory)
                    shirtAdd = Math.Min(shirtAdd + 1, (int)n.Shirts);
                else
                    shirtAdd2 = Math.Min(shirtAdd2 + 1, (int)n1.Shirts);
            }

            isGoodTrade = n1.IsGoodTrade(chocAdd, phoneAdd, shirtAdd, chocAdd2, phoneAdd2, shirtAdd2);
        }
        private void removeItemFromTrade(UIEventArg arg)
        {
            Nation n = world.GetPlayer();
            Nation n1 = world.Nations[selectedNation];
            if (arg.Component == tradeButtons[4])
            {
                if (isYourInventory)
                    chocAdd = Math.Max(chocAdd - 1, 0);
                else
                    chocAdd2 = Math.Max(chocAdd2 - 1, 0);
            }
            else if (arg.Component == tradeButtons[5])
            {
                if (isYourInventory)
                    phoneAdd = Math.Max(phoneAdd - 1, 0);
                else
                    phoneAdd2 = Math.Max(phoneAdd2 - 1, 0);
            }
            else if (arg.Component == tradeButtons[6])
            {
                if (isYourInventory)
                    shirtAdd = Math.Max(shirtAdd - 1, 0);
                else
                    shirtAdd2 = Math.Max(shirtAdd2 - 1, 0);
            }

            isGoodTrade = n1.IsGoodTrade(chocAdd, phoneAdd, shirtAdd, chocAdd2, phoneAdd2, shirtAdd2);
        }

        private void changeYourInventory(UIEventArg arg)
        {
            isYourInventory = true;
            tradeButtons[2].Background = Color.LightGreen;
            tradeButtons[3].Background = Color.Green;
        }
        private void changeTheirInventory(UIEventArg arg)
        {
            isYourInventory = false;
            tradeButtons[3].Background = Color.LightGreen;
            tradeButtons[2].Background = Color.Green;
        }

        private void selectNation1(UIEventArg arg)
        {
            if (selectedNation == 1)
                return;
            ResetTrade();
            selectedNation = 1;
            Nation n1 = world.Nations[selectedNation];
            isGoodTrade = n1.IsGoodTrade(chocAdd, phoneAdd, shirtAdd, chocAdd2, phoneAdd2, shirtAdd2);
        }

        private void selectNation2(UIEventArg arg)
        {
            if (selectedNation == 2)
                return;
            ResetTrade();
            selectedNation = 2;
            Nation n1 = world.Nations[selectedNation];
            isGoodTrade = n1.IsGoodTrade(chocAdd, phoneAdd, shirtAdd, chocAdd2, phoneAdd2, shirtAdd2);
        }

        private void OnNextDayClick(UIEventArg arg)
        {
            isTransitioning = true;
            isUiActive = false;
            state = GameTransitionState.FADE_IN;
            //world.ProgressDay();
        }

        private void OnTradeClick(UIEventArg arg)
        {
            form = UserInterfaceForms.TRADE;
        }

        private void OnLogisticsClick(UIEventArg arg)
        {
            form = UserInterfaceForms.LOGISTICS;

        }

        private void OnProductionClick(UIEventArg arg)
        {
            form = UserInterfaceForms.PRODUCTION;
        }

        /// <summary>
        /// Unloads the contents of the <c>GameScree</c>.
        /// </summary>
        public override void Unload()
        {
            Content.Unload();
        }

        private bool isGameOver = false;

        /// <summary>
        /// Updates the instance fo the <c>GameScreen</c>.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            manager.Update();

            Tile.Update(gameTime);
            world.Update(gameTime);

            foreach (UIButton button in productionBtns)
                button.Update(gameTime);

            if (isUiActive)
            {
                switch (form)
                {
                    case UserInterfaceForms.GAMEWORLD:
                        foreach (UIButton button in gameworldBtns)
                            button.Update(gameTime);
                        break;
                    case UserInterfaceForms.END_DAY:
                        if (InputManager.Instance.IsMouseClicked(MouseButton.RIGHT) ||
                            InputManager.Instance.IsMouseClicked(MouseButton.LEFT))
                        {
                            if (world.Day < GameWorld.RULE_DAYS)
                            {
                                isUiActive = false;
                                state = GameTransitionState.FADE_OUT;
                                form = UserInterfaceForms.GAMEWORLD;
                            }
                            else
                            {
                                isGameOver = true;
                            }
                        }
                        break;
                    case UserInterfaceForms.LOGISTICS:

                        break;
                    case UserInterfaceForms.PRODUCTION:
                        break;
                    case UserInterfaceForms.TRADE:
                        foreach (UIButton button in tradeButtons)
                            button.Update(gameTime);
                        break;
                }
            }
            else if (isTransitioning)
            {
                switch (state)
                {
                    case GameTransitionState.DO_STUFF:
                        if (!isUiActive)
                        {
                            world.ProgressDay();
                            isUiActive = true;
                            WorldSave save = world.CreateSave();
                            GameSerializer.Serialize(save, "Save/Load.sav");
                        }
                        form = UserInterfaceForms.END_DAY;
                        break;
                    case GameTransitionState.FADE_IN:
                        opacity += FADE_INTERVAL;
                        if (opacity > 1)
                        {
                            state = GameTransitionState.DO_STUFF;
                        }
                        break;
                    case GameTransitionState.FADE_OUT:
                        if (opacity <= 0)
                        {
                            isUiActive = true;
                            isTransitioning = false;
                        }
                        opacity -= FADE_INTERVAL;
                        break;
                }
            }

            if (InputManager.Instance.KeyPressed(Keys.Escape) && !form.Equals(UserInterfaceForms.END_DAY))
            {
                ResetTrade();
                form = UserInterfaceForms.GAMEWORLD;
            }

        }

        private void ResetTrade()
        {
            chocAdd = chocAdd2 = phoneAdd = phoneAdd2 = shirtAdd = shirtAdd2 = 0;
        }

        /// <summary>
        /// Draws the contents of the <c>GameScreen</c>.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.PointWrap, null, null, null, null);

            if (!isGameOver)
            {
                waterMap.Draw(spriteBatch);

                world.Draw(spriteBatch);

                Rectangle formRectangle = new Rectangle(16, 16, game.Width - 32, game.Height - 32);

                spriteBatch.Draw(blank, new Rectangle(0, 0, game.Width, game.Height), Color.Black * opacity);
                if (isUiActive)
                {
                    switch (form)
                    {
                        case UserInterfaceForms.GAMEWORLD:
                            DrawGameWorld(formRectangle, spriteBatch);
                            if (!world.GetPlayer().IsProducing())
                            {
                                string prodwar = "You are currently not producing anything";
                                float opacity = (float)(Math.Cos(timer / 400f) * 0.2f + 0.8f);
                                Vector2 prodwarSize = fontb.MeasureString(prodwar);
                                spriteBatch.DrawString(fontb, prodwar, new Vector2(game.Width - prodwarSize.X - 10, 10), Color.Black * opacity * 0.4f);
                                spriteBatch.DrawString(fontb, prodwar, new Vector2(game.Width - prodwarSize.X - 8, 8), Color.White * opacity);
                            }
                            else
                                timer = 0f;
                            break;
                        case UserInterfaceForms.END_DAY:
                            DrawEndDayScreen(spriteBatch);
                            break;

                        case UserInterfaceForms.LOGISTICS:
                            DrawLogistics(formRectangle, spriteBatch);
                            break;

                        case UserInterfaceForms.PRODUCTION:
                            DrawProduction(formRectangle, spriteBatch);
                            break;

                        case UserInterfaceForms.TRADE:
                            DrawTrade(formRectangle, spriteBatch);
                            break;
                    }
                }

            }
            else
            {
                Nation player = world.GetPlayer();
                string gameOverText = "Game Over!";
                int gdp = (int)(player.CalculateTotal(Goods.CHOCOLATE) + player.CalculateTotal(Goods.FACTORY)
                    + player.CalculateTotal(Goods.PHONE) + player.CalculateTotal(Goods.SHIRT)
                    + player.CalculateTotal(Goods.TOOL) + player.CalculateTotal(Goods.TRUCK));
                string gdptext = "Your leadership ended with an economy worth $" + gdp;
                Vector2 gameOvTextSize = fontb.MeasureString(gameOverText);
                Vector2 gdpTextSize = font.MeasureString(gdptext);
                spriteBatch.DrawString(fontb, gameOverText, 
                    new Vector2(game.Width / 2 - gameOvTextSize.X / 2, 16), Color.Black);
                spriteBatch.DrawString(font, gdptext, new Vector2(game.Width / 2 - gdpTextSize.X / 2, 32 + gameOvTextSize.Y), Color.Black);
                string pelkText = "";

                Texture2D pelkTexture = null;
                Texture2D endTexture = null;
                if (gdp >= 10000)
                {
                    pelkTexture = pelkSmile;
                    pelkText = "You're the one!";
                    endTexture = apIcons[4];
                }
                else if (gdp < 10000 && gdp > 9000)
                {
                    pelkTexture = pelkSmile;
                    pelkText = "You've got potential";
                    endTexture = apIcons[3];
                }
                else if (gdp <= 9000 && gdp > 8000)
                {
                    pelkTexture = pelkSmile;
                    pelkText = "Congratulation, you passed the class.";
                    endTexture = apIcons[2];
                }
                else if (gdp <= 8000 && gdp > 6000)
                {
                    pelkTexture = pelkFrown;
                    pelkText = "Don't worry. At The People's Republic of Berkeley,\n we give you shovel. It's glorious!";
                    endTexture = apIcons[1];
                }
                else
                {
                    pelkTexture = pelkFrown;
                    pelkText = "You were supposed to be the one, not get a one...";
                    endTexture = apIcons[0];
                }
                float scale = game.Width / endTexture.Width;
                Rectangle endRect = new Rectangle((int)(game.Width / 2 - endTexture.Width * scale / 2),
                    (int)(game.Height / 2 - endTexture.Height * scale / 2), (int)(endTexture.Width * scale),
                    (int)(endTexture.Height * scale));
                spriteBatch.Draw(endTexture, endRect, Color.White);
                
                spriteBatch.Draw(pelkTexture, new Rectangle(328, endRect.Y + endRect.Height / 2 + 64,
                    pelkTexture.Width, pelkTexture.Height), Color.White);
                spriteBatch.DrawString(fontb, pelkText, new Vector2(336 + pelkTexture.Width, endRect.Height / 4 + 
                    endRect.Y + pelkTexture.Height), Color.Black);
            }
            spriteBatch.End();
        }

        private void DrawLogistics(Rectangle formRectangle, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(blank, formRectangle, Color.DarkGreen);
            Nation player = world.GetPlayer();
            float choc = player.Production / Nation.CHOC_NEEDED * player.ChocolateAdvantage;
            float phone = player.Production / Nation.PHONE_NEEDED * player.PhoneAdvantage;
            float shirt = player.Production / Nation.SHIRT_NEEDED * player.ShirtAdvantage;

            int dim = 200;
            int sep = (game.Height - 3 * dim) / 4;
            string highestAdvantage = "";
            string advice = "";
            if (player.ChocolateAdvantage == 1)
            {
                highestAdvantage = "chocolates";
            }
            else if (player.PhoneAdvantage == 1)
            {
                highestAdvantage = "phones";
            }
            else if (player.ShirtAdvantage == 1)
            {
                highestAdvantage = "shirts";
            }
            advice = "You have the highest comparative advantage for producing " + highestAdvantage
                + " I advise you\n spend most of your production producing them and trading them for other goods.\n";

            string ppcSummary = "According to our production possibility graphs, by " +
                "specializing and focusing all production\nand resources towards producing:\n\n" +
                "Chocolates -> You can make " + choc + " each day. \n" + "Phones -> You can make "
                + phone + " each day. \n" + "Shirts -> You can make " + shirt + " each day.\n";

            string warning = "Be careful about producing too much of one item. \nThe less scarce an item is\nThe less it's worth";

            string text = "Economic Advisor:\n\n" + advice + "\n\n" + ppcSummary + "\n\n" + warning;

            spriteBatch.DrawString(font, text, new Vector2(3 * dim, 64), Color.White);

            Graph ppChocPhone = new Graph(game.GraphicsDevice, new Point(dim, dim))
            {
                Position = new Vector2(200, dim + sep),
                MaxValue = phone * 1.5f,
            };
            List<float> ppChocPhoneVals = new List<float>
            {
                phone,
                0
            };
            Graph ppChocShirt = new Graph(game.GraphicsDevice, new Point(dim, dim))
            {
                MaxValue = shirt * 1.5f,
                Position = new Vector2(200, dim * 2 + 2 * sep)
            };
            List<float> ppChocShirtVals = new List<float>
            {
                shirt,
                0
            };
            Graph ppPhoneShirt = new Graph(game.GraphicsDevice, new Point(dim, dim))
            {
                Position = new Vector2(200, dim * 3 + 3 * sep),
                MaxValue = shirt * 1.5f
            };
            List<float> ppPhoneShirtVals = new List<float>
            {
                shirt,
                0
            };
            int width = 5;
            string ppChocText = "Chocolates (" + choc + ")";
            string ppPhoneText = "Phones (" + phone + ")";
            string ppShirtText = "Shirts (" + shirt + ")";
            spriteBatch.DrawString(font, ppPhoneText, new Vector2(3 * dim / 2 - 
                font.MeasureString(ppPhoneText).X / 2, ppChocPhone.Position.Y + 6 + width), Color.White);
            spriteBatch.DrawString(font, ppShirtText, new Vector2(3 * dim / 2 - 
                font.MeasureString(ppShirtText).X / 2, ppPhoneShirt.Position.Y + 6 + width), Color.White);
            spriteBatch.DrawString(font, ppShirtText, new Vector2(3 * dim / 2 - 
                font.MeasureString(ppShirtText).X / 2, ppChocShirt.Position.Y + 6 + width), Color.White);
            spriteBatch.DrawString(font, ppChocText, new Vector2(200 - font.MeasureString(ppChocText).X - 6,
                ppChocPhone.Position.Y - dim / 2 - font.MeasureString(ppChocText).Y / 2), Color.White);
            spriteBatch.DrawString(font, ppPhoneText, new Vector2(200 - font.MeasureString(ppPhoneText).X - 6,
                ppPhoneShirt.Position.Y - dim / 2 - font.MeasureString(ppChocText).Y / 2), Color.White);
            spriteBatch.DrawString(font, ppChocText, new Vector2(200 - font.MeasureString(ppChocText).X - 6,
                ppChocShirt.Position.Y - dim / 2 - font.MeasureString(ppChocText).Y / 2), Color.White);
            //spriteBatch.DrawString(font, "Phones", new Vector2(), Color.White);
            //spriteBatch.DrawString(font, "Chocolates", new Vector2(), Color.White);
            DrawBorder(new Rectangle(200 - width, -width + sep, 200 + 2 * width, 200 + 2 *
                width), width, Color.White, spriteBatch);
            DrawBorder(new Rectangle(200 - width, sep * 2 + dim - width, 200 + 2 * width,
                200 + 2 * width), width, Color.White, spriteBatch);
            DrawBorder(new Rectangle(200 - width, 3 * sep + 2 * dim - width, 200 + 2 * width,
                200 + 2 * width), width, Color.White, spriteBatch);
            //DrawBorder(new Rectangle(200, 300, 200, 200), width, Color.White, spriteBatch);
            //DrawBorder(new Rectangle(200, 600, 200, 200), width, Color.White, spriteBatch);
            ppChocPhone.Draw(ppChocPhoneVals, Color.Silver);
            ppPhoneShirt.Draw(ppPhoneShirtVals, Color.White);
            ppChocShirt.Draw(ppChocShirtVals, Color.White);
        }

        private void DrawEndDayScreen(SpriteBatch spriteBatch)
        {
            Nation player = world.GetPlayer();
            string dayText = (world.Day < GameWorld.RULE_DAYS) ? "Day " + world.Day + " of " 
                + GameWorld.RULE_DAYS : "Final Day";
            dayText += " - " + world.GetPlayer().Name;
            Vector2 dayTextSize = fontb.MeasureString(dayText);
            spriteBatch.DrawString(fontb, dayText, new Vector2(game.Width / 2 - dayTextSize.X / 2, 16), Color.White);
            DayInfo info = world.LastDayStats;
            
            int chocProduced = (int)info.ProducedChocolate;
            int chocProducedM = (int)player.CalculateValue(Goods.CHOCOLATE, 0, chocProduced);
            int chocBought = (int)info.BoughtChocolate;
            int chocBoughtM = (int)player.CalculateValue(Goods.CHOCOLATE, chocProduced, chocProduced + chocBought);
            int chocSold = (int)info.TradeChocolate;
            int chocSoldM = (int)player.CalculateValue(Goods.CHOCOLATE, chocProduced +
                chocBought, chocProduced + chocBought + chocSold);
            int shirtProduced = (int)info.ProducedShirts;
            int shirtProducedM = (int)player.CalculateValue(Goods.SHIRT, 0, shirtProduced);
            int shirtBought = (int)info.BoughtShirt;
            int shirtBoughtM = (int)player.CalculateValue(Goods.SHIRT, shirtProduced, shirtProduced + shirtBought);
            int shirtSold = (int)info.TradeShirt;
            int shirtSoldM = (int)player.CalculateValue(Goods.SHIRT, shirtProduced +
                shirtBought, shirtProduced + shirtBought + shirtSold);
            int phoneProduced = (int)info.ProducedPhones;
            int phoneProducedM = (int)player.CalculateValue(Goods.PHONE, 0, phoneProduced);
            int phoneBought = (int)info.BoughtPhone;
            int phoneBoughtM = (int)player.CalculateValue(Goods.PHONE, phoneProduced, phoneProduced + phoneBought);
            int phoneSold = (int)info.TradePhone;
            int phoneSoldM = (int)player.CalculateValue(Goods.PHONE, phoneProduced +
                phoneBought, phoneProduced + phoneBought + phoneSold);
            //(int)(chocProduced * Nation.CHOC_NEEDED / player.ChocolateAdvantage);
            //(int)(chocBought * Nation.CHOC_NEEDED / player.ChocolateAdvantage);
            //(int)(chocSold * Nation.CHOC_NEEDED / player.ChocolateAdvantage);

            //int shirtProduced = (int)info.ProducedShirts;
            //int shirtProducedM = (int)(shirtProduced * Nation.SHIRT_NEEDED / player.ShirtAdvantage);
            //int shirtBought = (int)info.BoughtShirt;
            //int shirtBoughtM = (int)(shirtBought * Nation.SHIRT_NEEDED / player.ShirtAdvantage);
            //int shirtSold = (int)info.TradeShirt;
            //int shirtSoldM = (int)(shirtSold * Nation.SHIRT_NEEDED / player.ShirtAdvantage);

            //int phoneProduced = (int)info.ProducedPhones;
            //int phoneProducedM = (int)(phoneProduced * Nation.PHONE_NEEDED / player.PhoneAdvantage);
            //int phoneBought = (int)info.BoughtPhone;
            //int phoneBoughtM = (int)(phoneBought * Nation.PHONE_NEEDED / player.PhoneAdvantage);
            //int phoneSold = (int)info.TradePhone;
            //int phoneSoldM = (int)(phoneSold * Nation.PHONE_NEEDED / player.PhoneAdvantage);
            string consumertext = "Chocolates\nOwned: "
                + chocProduced + " = $" + chocProducedM +
                "\nSold: " + chocSold + " = $" + chocSoldM + "\nBought: " + chocBought + " = -$"
                + chocBoughtM + "\n"
                + "Shirts\nOwned: " + shirtProduced + " = $" + shirtProducedM +
                "\nSold: " + shirtSold + " = $" + shirtSoldM + "\nBought: " + shirtBought + " = -$"
                + shirtBoughtM + "\n"
                + "Phones\nOwned: " + phoneProduced + " = $" + phoneProducedM +
                "\nSold: " + phoneSold + " = $" + phoneSoldM + "\nBought: " + phoneBought + " = -$"
                + phoneBoughtM;

            int factories = (int)(info.ProducedFactories);
            int factoryM = (int)player.CalculateValue(Goods.FACTORY, 0, factories);//(int)(factories * Nation.FACTORY_NEEDED);
            int trucks = (int)(info.ProducedTrucks);
            int truckM = (int)player.CalculateValue(Goods.TRUCK, 0, trucks); //(int)(trucks * Nation.TRUCK_NEEDED);
            int tools = (int)(info.ProducedTools);
            int toolM = (int)player.CalculateValue(Goods.TOOL, 0, tools);//(int)(tools * Nation.TOOL_NEEDED);

            string capitalText = "Factories: " + factories + " = $" + factoryM +
                "\n\n\n\n\n\nTrucks: " + trucks + " = $" + truckM + "\n\n\n\n\nTools: " + tools + " = $" + toolM;
            Vector2 capSize = font.MeasureString(capitalText);
            Vector2 conSize = font.MeasureString(consumertext);
            float totalWidth = capSize.X + conSize.X + 32;
            float x = game.Width / 2 - totalWidth / 2;

            spriteBatch.DrawString(font, consumertext, new Vector2(x, dayTextSize.Y + 16), Color.White);
            spriteBatch.DrawString(font, capitalText, new Vector2(x + conSize.X + 32, dayTextSize.Y + 16), Color.White);

            //int gdp = (int)((shirtProducedM + chocProducedM + phoneProducedM + factoryM + truckM
            //    + toolM - chocSoldM - shirtSoldM - phoneSoldM + chocBoughtM + shirtBoughtM + phoneBoughtM));
            int gdp = (int)(player.CalculateTotal(Goods.CHOCOLATE) + player.CalculateTotal(Goods.FACTORY)
                + player.CalculateTotal(Goods.PHONE) + player.CalculateTotal(Goods.SHIRT)
                + player.CalculateTotal(Goods.TOOL) + player.CalculateTotal(Goods.TRUCK));

            string gdpText = "Real Gross Domestic Production = C + I + G + Xn = $"
                + (shirtProducedM + chocProducedM + phoneProducedM) + " + $" +
                (factoryM + truckM + toolM) + " + $0 + ($" + (chocBoughtM + shirtBoughtM + phoneBoughtM)
                + " - $" + (chocSoldM + shirtSoldM + phoneSoldM) + ") = $" + gdp;
            Vector2 gdpTextSize = font.MeasureString(gdpText);
            spriteBatch.DrawString(font, gdpText, new Vector2(game.Width / 2 - gdpTextSize.X / 2
                , game.Height - gdpTextSize.Y), Color.White);
            DayInfo currentDay = player.DayStats[player.DayStats.Count - 1];
            DayInfo prevDay = player.DayStats[player.DayStats.Count - 2];
            float productionGrowth = currentDay.Production - prevDay.Production;
            float populationGrowth = (int)(currentDay.Population - prevDay.Population);
            float unemployment = player.Unemployment;

            string prodGrow = (productionGrowth > 0) ? "+" + productionGrowth : "-" + productionGrowth;
            if (productionGrowth == 0)
                prodGrow = "None";
            string popGrow = (populationGrowth > 0) ? "+" + populationGrowth : "-" + populationGrowth;
            if (populationGrowth == 0)
                popGrow = "None";
            string endDayText = "Production Growth: " + prodGrow + "\n";
            endDayText += "Population Growth: " + popGrow + "\n";
            endDayText += "Unemployment Rate: %" + (unemployment * 100f);
            Vector2 endDaytextSize = font.MeasureString(endDayText);
            spriteBatch.DrawString(font, endDayText, new Vector2(game.Width / 2 - endDaytextSize.X / 2,
                conSize.Y + 72), Color.White);
        }

        private void DrawGameWorld(Rectangle formRectangle, SpriteBatch spriteBatch)
        {
            Nation n = world.GetPlayer();
            string text = n.Name + "'s Economy - Day " + world.Day
                + "\nPopulation: " + n.Population + "\nCoco Beans: " + n.Coco + "\nSilk: " 
                + n.Silk + "\nMaterials: " + n.Materials + "";
            foreach (UIButton button in gameworldBtns)
                button.Draw(spriteBatch);
            spriteBatch.DrawString(font, text, new Vector2(9, 11), Color.Black * 0.5f);
            spriteBatch.DrawString(font, text, new Vector2(10, 10), Color.White);

        }
        private void OnProductionBtnClick(UIEventArg arg)
        {
            Nation n = world.GetPlayer();
            if (arg.Component == productionBtns[0])
            {
                n.QueuedFactories++;
            }
            else if (arg.Component == productionBtns[1])
            {
                n.QueuedTrucks++;
            }
            else if (arg.Component == productionBtns[2])
            {
                n.QueuedTools++;
            }
            else if (arg.Component == productionBtns[3])
            {
                n.QueuedChocolates++;
            }
            else if (arg.Component == productionBtns[4])
            {
                n.QueuedShirts++;
            }
            else if (arg.Component == productionBtns[5])
            {
                n.QueuedPhones++;
            }
            else if (arg.Component == productionBtns[6])
            {
                n.QueuedFactories = Math.Max(0, n.QueuedFactories - 1);
            }
            else if (arg.Component == productionBtns[7])
            {
                n.QueuedTrucks = Math.Max(0, n.QueuedTrucks - 1);
            }
            else if (arg.Component == productionBtns[8])
            {
                n.QueuedTools = Math.Max(0, n.QueuedTools - 1);
            }
            else if (arg.Component == productionBtns[9])
            {
                // choco
                n.QueuedChocolates = Math.Max(0, n.QueuedChocolates - 1);
            }
            else if (arg.Component == productionBtns[10])
            {
                // shirt
                n.QueuedShirts = Math.Max(0, n.QueuedShirts - 1);
            }
            else if (arg.Component == productionBtns[11])
            {
                // phone
                n.QueuedPhones = Math.Max(0, n.QueuedPhones - 1);
            }
        }

        private Rectangle DrawForm(Rectangle formRectangle, SpriteBatch spriteBatch, string title)
        {
            Nation n = world.GetPlayer();
            spriteBatch.Draw(blank, formRectangle, Color.Green);
            Vector2 titlePos = fontb.MeasureString(title);
            Rectangle activeRect = new Rectangle(formRectangle.X, (int)(formRectangle.Y + titlePos.Y * 2),
                formRectangle.Width, (int)(formRectangle.Height - titlePos.Y * 2));
            spriteBatch.DrawString(fontb, title, new Vector2(game.Width / 2 - titlePos.X / 2, formRectangle.Y + 12), Color.White);

            return activeRect;
        }

        private void DrawTrade(Rectangle formRectangle, SpriteBatch spriteBatch)
        {
            Nation n = world.GetPlayer();
            Rectangle activeRect = DrawForm(formRectangle, spriteBatch, "Trade");
            Vector2 nameSize = fontb.MeasureString(world.Nations[selectedNation].Name);
            Vector2 pos = new Vector2(activeRect.X + activeRect.Width / 2 - nameSize.X / 2, activeRect.Y - 16);

            Rectangle fRect = new Rectangle(formRectangle.X + 8, (int)(pos.Y + nameSize.Y),
                formRectangle.Width - 16, (int)(activeRect.Height - 56));
            Rectangle fRect2 = new Rectangle(fRect.X + fRect.Width / 2, fRect.Y, fRect.Width / 2, fRect.Height);
            Rectangle fRect3 = new Rectangle(fRect.X + fRect.Width / 2, fRect.Y + fRect.Height / 2, fRect.Width / 2, fRect.Height / 2);
            spriteBatch.Draw(blank, fRect, Color.DarkGreen);
            spriteBatch.Draw(blank, fRect2, new Color(0, 91, 5));
            spriteBatch.DrawString(fontb, "Your items:", new Vector2(fRect2.X + 8, fRect2.Y + 8), Color.White);
            spriteBatch.Draw(blank, fRect3, new Color(0, 83, 4));
            spriteBatch.DrawString(fontb, "Their items:", new Vector2(fRect3.X + 8, fRect3.Y + 8), Color.White);
            spriteBatch.DrawString(fontb, world.Nations[selectedNation].Name,
                pos - new Vector2(2, -2), Color.Black * 0.6f);
            spriteBatch.DrawString(fontb, world.Nations[selectedNation].Name, pos, Color.White);
            tradeButtons[0].Position = new Point(formRectangle.X + 8, formRectangle.Y + 8);
            tradeButtons[1].Position = new Point(formRectangle.X + 80, formRectangle.Y + 8);
            tradeButtons[2].Position = new Point(fRect.X + 4, fRect.Y + 4);
            tradeButtons[3].Position = new Point(fRect.X + 230, fRect.Y + 4);

            int height = 128;
            int space = (fRect.Height - 64 - 4 * height) / 4;

            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(blank, new Rectangle(tradeButtons[4 + i].Position.X,
                    tradeButtons[4 + i].Position.Y, fRect.Width / 2 - 32, height),
                    new Color(0, 81, 4));

                tradeButtons[4 + i].Position = new Point(tradeButtons[2].Position.X,
                    fRect.Y + 64 + (i + 1) * (space) + height * i);

                tradeButtons[4 + i].Size = new Point(height, height);
                int sel = selectedNation;

                // choc, ph, sh
                Nation nat = world.Nations[sel];
                
                string text = "";
                if (i == 0)
                {
                    int chocolates = (isYourInventory) ? (int)n.Chocolates : (int)nat.Chocolates;
                    text = "Chocolate\nQuantity: " + chocolates;
                    if (nat != n)
                    {
                        text += "\nTheir Advantage: " + nat.ChocolateAdvantage + 
                            "\nYour Advantage: " + n.ChocolateAdvantage;
                    }
                }
                else if (i == 1)
                {
                    int phones = (isYourInventory) ? (int)n.Phones : (int)nat.Phones;
                    text = "Phone\nQuantity: " + phones;
                    if (nat != n)
                    {
                        text += "\nTheir Advantage: " + nat.PhoneAdvantage +
                            "\nYour Advantage: " + n.PhoneAdvantage;
                    }
                }
                else if (i == 2)
                {
                    int shirts = (isYourInventory) ? (int)n.Shirts : (int)nat.Shirts;
                    text = "Shirt\nQuantity: " + shirts;
                    if (nat != n)
                    {
                        text += "\nTheir Advantage: " + nat.ShirtAdvantage +
                            "\nYour Advantage: " + n.ShirtAdvantage;
                    }
                }
                spriteBatch.DrawString(font, text, new Vector2(tradeButtons[4 + i].Position.X - 3
                    + height + 8, tradeButtons[4 + i].Position.Y + 11), Color.Black * 0.5f);
                spriteBatch.DrawString(font, text, new Vector2(tradeButtons[4 + i].Position.X
                    + height + 8, tradeButtons[4 + i].Position.Y + 8), Color.White);

                int height2 = 74;
                int spacing2 = (fRect2.Width - 5 * height2) / 6;

                int num0 = 0;
                int num1 = 0;
                for (int j = num0; j < (int)chocAdd + num1; j++)
                {
                    int y = num0 / 5;

                    spriteBatch.Draw(goodIcons[3], new Rectangle(fRect2.X + spacing2 * (j + 1 - y * 5)
                        + height2 * (j - y * 5), fRect.Y + space * (y + 1) + height2 * y + 8, height2, height2), Color.White);

                    num0++;
                }
                num1 = num0;
                for (int j = num0; j < (int)shirtAdd + num1; j++)
                {
                    int y = num0 / 5;

                    spriteBatch.Draw(goodIcons[4], new Rectangle(fRect2.X + spacing2 * (j + 1 - y * 5)
                        + height2 * (j - y * 5), fRect.Y + space * (y + 1) + height2 * y + 8, height2, height2), Color.White);

                    num0++;
                }
                num1 = num0;
                for (int j = num0; j < (int)phoneAdd + num1; j++)
                {
                    int y = num0 / 5;

                    spriteBatch.Draw(goodIcons[5], new Rectangle(fRect2.X + spacing2 * (j + 1 - y * 5)
                        + height2 * (j - y * 5), fRect.Y + space * (y + 1) + height2 * y + 8, height2, height2), Color.White);

                    num0++;
                }

                int theirSpace = fRect.Height / 2 + 16;
                num0 = 0;
                num1 = 0;
                for (int j = num0; j < (int)chocAdd2 + num1; j++)
                {
                    int y = num0 / 5;

                    spriteBatch.Draw(goodIcons[3], new Rectangle(fRect2.X + spacing2 * (j + 1 - y * 5)
                        + height2 * (j - y * 5), theirSpace + fRect.Y + space * (y + 1) + height2 * y, height2, height2), Color.White);

                    num0++;
                }
                num1 = num0;
                for (int j = num0; j < (int)shirtAdd2 + num1; j++)
                {
                    int y = num0 / 5;

                    spriteBatch.Draw(goodIcons[4], new Rectangle(fRect2.X + spacing2 * (j + 1 - y * 5)
                        + height2 * (j - y * 5), theirSpace + fRect.Y + space * (y + 1) + height2 * y, height2, height2), Color.White);

                    num0++;
                }
                num1 = num0;
                for (int j = num0; j < (int)phoneAdd2 + num1; j++)
                {
                    int y = num0 / 5;

                    spriteBatch.Draw(goodIcons[5], new Rectangle(fRect2.X + spacing2 * (j + 1 - y * 5)
                        + height2 * (j - y * 5), theirSpace + fRect.Y + space * (y + 1) + height2 * y, height2, height2), Color.White);

                    num0++;
                }
            }

            tradeButtons[7].Position = new Point(fRect.X + fRect.Width / 2 - 180,
                fRect.Y + fRect.Height - 72);

            string tradeText = (isGoodTrade) ? "This trade deal can work." : "This is not a fair trade deal.";
            //int moneyMade = (int)((chocAdd2 - chocAdd) * Nation.CHOC_NEEDED / n.ChocolateAdvantage
            //    + (phoneAdd2 - phoneAdd) * Nation.PHONE_NEEDED / n.PhoneAdvantage 
            //    + (shirtAdd2 - shirtAdd) * Nation.SHIRT_NEEDED / n.ShirtAdvantage);
            Nation player = world.GetPlayer();

            int dChoc = (int)(chocAdd2 - chocAdd);
            int dPhone = (int)(phoneAdd2 - phoneAdd);
            int dShirt = (int)(shirtAdd2 - shirtAdd);

            double chocMoney = (dChoc >= 0) ? player.CalculateValue(Goods.CHOCOLATE,
                (int)player.Chocolates, (int)player.Chocolates + dChoc) :
                -player.CalculateValue(Goods.CHOCOLATE, (int)player.Chocolates + dChoc,
                (int)player.Chocolates);
            double phoneMoney = (dPhone >= 0) ? player.CalculateValue(Goods.PHONE,
                (int)player.Phones, (int)player.Phones + dPhone) :
                -player.CalculateValue(Goods.PHONE, (int)player.Phones + dPhone,
                (int)player.Phones);
            double shirtMoney = (dShirt >= 0) ? player.CalculateValue(Goods.SHIRT,
                (int)player.Shirts, (int)player.Shirts + dShirt) :
                -player.CalculateValue(Goods.SHIRT, (int)player.Shirts + dShirt,
                (int)player.Shirts);

            int moneyMade = (int)Math.Round(chocMoney + phoneMoney + shirtMoney);
            string tradeText2 = world.Nations[selectedNation].Name + ": " + tradeText;
            Vector2 tradeTextSize = font.MeasureString(tradeText2);
            Color c = (moneyMade > 0) ? Color.LimeGreen : Color.Red;
            string moneyText = (moneyMade > 0) ? "Gaining $" + moneyMade : "Losing $" + Math.Abs(moneyMade);
            spriteBatch.DrawString(font, tradeText2, new Vector2(fRect.X + 8, fRect.Y + fRect.Height - 72), Color.White);

            if (Math.Abs(moneyMade) > 0)
                spriteBatch.DrawString(font, moneyText, new Vector2(fRect.X + 8, fRect.Y + fRect.Height - 72 + tradeTextSize.Y), c);

            foreach (UIButton butt in tradeButtons)
            {
                butt.Draw(spriteBatch);
            }

        }

        private void DrawProduction(Rectangle formRectangle, SpriteBatch spriteBatch)
        {
            Nation n = world.GetPlayer();
            Rectangle activeRect = DrawForm(formRectangle, spriteBatch, "Production");
            Vector2 titlePos = fontb.MeasureString("Production");
            //spriteBatch.Draw(blank, formRectangle, Color.Green);
            //Vector2 titlePos = fontb.MeasureString("Production");
            //Rectangle activeRect = new Rectangle(formRectangle.X, (int)(formRectangle.Y + titlePos.Y * 2), formRectangle.Width, (int)(formRectangle.Height - titlePos.Y * 2));
            //spriteBatch.DrawString(fontb, "Production", new Vector2(game.Width / 2 - titlePos.X / 2, formRectangle.Y + 12), Color.White);

            Vector2 capitalPos = fontb.MeasureString("Capital");
            Vector2 consumerPos = fontb.MeasureString("Consumer");

            spriteBatch.DrawString(fontb, "Capital", new Vector2(formRectangle.X + 
                formRectangle.Width / 4 - capitalPos.X / 2, formRectangle.Y
                + titlePos.Y * 2), Color.White);
            spriteBatch.DrawString(fontb, "Consumer", new Vector2(formRectangle.X + 3 *
                formRectangle.Width / 4 - consumerPos.X / 2, formRectangle.Y 
                + titlePos.Y * 2), Color.White);

            int spacing = 72;
            int height = (activeRect.Height - 4 * spacing) / 3;
            int width = activeRect.Width / 2 - 124;
            int buttonDim = height - 16;
            for (int i = 0; i < 3; i++)
            {
                Rectangle rect0 = new Rectangle(activeRect.X + 16, activeRect.Y
                    + spacing * (i + 1) + i * height, width, height);
                Rectangle rect1 = new Rectangle(activeRect.X + activeRect.Width
                    - width - 16, activeRect.Y + spacing * (i + 1) + i * height, 
                    width, height);
                productionBtns[i].Size = new Point(buttonDim, buttonDim);
                productionBtns[i].Position = new Point(rect0.X + rect0.Width - 
                    buttonDim - 8, rect0.Y + 8);
                productionBtns[i + 3].Size = new Point(buttonDim, buttonDim);
                productionBtns[i + 3].Position = new Point(rect1.X + rect1.Width - 8 -
                    buttonDim, rect1.Y + 8);
                productionBtns[i + 6].Size = new Point(buttonDim, buttonDim);
                productionBtns[i + 6].Position = new Point(rect0.X + rect0.Width -
                    buttonDim * 2 - 16, rect0.Y + 8);
                productionBtns[i + 9].Size = new Point(buttonDim, buttonDim);
                productionBtns[i + 9].Position = new Point(rect1.X + rect1.Width - 
                    buttonDim * 2 - 16, rect1.Y + 8);
                spriteBatch.Draw(blank, rect0, Color.DarkGreen);
                spriteBatch.Draw(goodIcons[i], new Rectangle(rect0.X + 2, rect0.Y + 2,
                    rect0.Height - 4,rect0.Height - 4), Color.White);
                spriteBatch.Draw(blank, rect1, Color.DarkGreen);
                spriteBatch.Draw(goodIcons[i + 3], new Rectangle(rect1.X + 2, rect1.Y + 2,
                    rect0.Height - 4, rect0.Height - 4), Color.White);

                // itemname
                // quantity: #
                // queued: #
                string capitalText = "";
                if (i == 0)
                {
                    string percent = (n.QueuedFactories > 0 && n.QueuedFactories != 
                        (int)n.QueuedFactories) ? (int)((1 - (n.QueuedFactories 
                        - (int)n.QueuedFactories)) * 100) + "%" : "None";
                    capitalText = "Factory\nQuantity: "
                        + (int)n.Factories + "\nQueued: " +
                        (int)Math.Ceiling(n.QueuedFactories) + "\nCompleted: " + percent;
                }
                else if (i == 1)
                {
                    string percent = (n.QueuedTrucks > 0 && n.QueuedTrucks != 
                        (int)n.QueuedTrucks) ? "" + (int)((1 - (n.QueuedTrucks 
                        - (int)n.QueuedTrucks)) * 100) + "%": "None";
                    capitalText = "Truck\nQuantity: " 
                        + (int)n.Trucks + "\nQueued: " +
                        (int)Math.Ceiling(n.QueuedTrucks) + "\nCompleted: " + percent;
                }
                else if (i == 2)
                {
                    string percent = (n.QueuedTools > 0 && n.QueuedTools 
                        != (int)n.QueuedTools) ? (int)((1 - (n.QueuedTools
                        - (int)n.QueuedTools)) * 100) + "%" : "None";
                    capitalText = "Set of Tools\nQuantity: " + (int)n.Tools + "\nQueued: " +
                        (int)Math.Ceiling(n.QueuedTools) + "\nCompleted: " + percent;
                }

                spriteBatch.DrawString(font, capitalText, new Vector2(rect0.X + height + 4,
                    productionBtns[i].Position.Y), Color.White);

                string consumerText = "";
                if (i == 0)
                {
                    string percent = (n.QueuedChocolates > 0 && n.QueuedChocolates != 
                        (int)n.QueuedChocolates) ? (int)((1 - (n.QueuedChocolates
                        - (int)n.QueuedChocolates)) * 100) + "%": "None";
                    consumerText = "Chocolate\nAdvantage: " + n.ChocolateAdvantage + 
                        "\nQuantity: " + (int)n.Chocolates + "\nQueued: " +
                        (int)Math.Ceiling(n.QueuedChocolates) + "\nCompleted: " + percent;
                }
                else if (i == 1)
                {
                    string percent = (n.QueuedShirts > 0 && n.QueuedShirts != (int)n.QueuedShirts)
                        ? (int)((1 - (n.QueuedShirts
                        - (int)n.QueuedShirts)) * 100) + "%" : "None";
                    consumerText = "Shirt\nAdvantage: " + n.ShirtAdvantage+ "\nQuantity: "
                        + (int)n.Shirts + "\nQueued: " +
                        (int)Math.Ceiling(n.QueuedShirts) + "\nCompleted: " + percent;
                }
                else if (i == 2)
                {
                    string percent = (n.QueuedPhones > 0 && n.QueuedPhones 
                        != (int)n.QueuedPhones) ? (int)((1 - (n.QueuedPhones 
                        - (int)n.QueuedPhones)) * 100) + "%" : "None";
                    consumerText = "Phone\nAdvantage: " + n.PhoneAdvantage + "\nQuantity: " 
                        + (int)n.Phones + "\nQueued: " +
                        (int)Math.Ceiling(n.QueuedPhones) + "\nCompleted: " + percent;
                }

                spriteBatch.DrawString(font, consumerText, new Vector2(rect1.X + height + 4, 
                    productionBtns[i].Position.Y), Color.White);
            }

            foreach (UIButton button in productionBtns)
                button.Draw(spriteBatch);
            
        }

        /// <summary>
        /// Will draw a border (hollow rectangle) of the given 'thicknessOfBorder' (in pixels)
        /// of the specified color.
        ///
        /// By Sean Colombo, from http://bluelinegamestudios.com/blog
        /// </summary>
        /// <param name="rectangleToDraw"></param>
        /// <param name="thicknessOfBorder"></param>
        private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor, SpriteBatch spriteBatch)
        {
            // Draw top line
            spriteBatch.Draw(blank, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            spriteBatch.Draw(blank, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            spriteBatch.Draw(blank, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            // Draw bottom line
            spriteBatch.Draw(blank, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }

    }

}
