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

namespace DiamondInTheWater.Screens
{
    public class GameScreen : Screen
    {
        private enum UserInterfaceForms
        {
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
        private Texture2D blank;
        private ContentManager Content;
        private UIButton[] gameworldBtns, productionBtns, tradeButtons;
        private Texture2D[] goodIcons;
        private float opacity;
        private bool isTransitioning, isYourInventory;
        private const float FADE_INTERVAL = 0.025f;
        private int selectedNation;
        private float chocAdd, phoneAdd, shirtAdd, chocAdd2, phoneAdd2, shirtAdd2;

        /// <summary>
        /// Creates a new instance of the <c>GameScreen</c>.
        /// </summary>
        public GameScreen(Game1 game)
        {
            selectedNation = 1;
            state = GameTransitionState.FADE_IN;
            isTransitioning = false;
            this.game = game;
            world = new GameWorld(game);
            isUiActive = true;
            isYourInventory = true;
            form = UserInterfaceForms.GAMEWORLD;
        }

        /// <summary>
        /// Initializes the <c>GameScreen</c>.
        /// </summary>
        /// <param name="Content"></param>
        public override void Initialize(ContentManager Content)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            goodIcons = new Texture2D[6];
            goodIcons[0] = Content.Load<Texture2D>("FactoryIcon");
            goodIcons[1] = Content.Load<Texture2D>("TruckIcon");
            goodIcons[2] = Content.Load<Texture2D>("ToolIcon");
            goodIcons[3] = Content.Load<Texture2D>("ChocolateIcon");
            goodIcons[4] = Content.Load<Texture2D>("ShirtIcon");
            goodIcons[5] = Content.Load<Texture2D>("PhoneIcon");

            font = this.Content.Load<SpriteFont>("defaultFont");
            blank = this.Content.Load<Texture2D>("blank");
            world.Initialize(this.Content);

            int size = Map.TILE_SCALE * 32;
            waterMap = new Map(game.Width / size + 1, game.Height / size + 1);

            fontb = this.Content.Load<SpriteFont>("largeFont");
            int space = (game.Width - 4 * 200) / 5;

            #region Game world form elements
            gameworldBtns = new UIButton[4];
            gameworldBtns[0] = new UIButton();
            gameworldBtns[1] = new UIButton();
            gameworldBtns[2] = new UIButton();
            gameworldBtns[3] = new UIButton();
        
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
            gameworldBtns[3].Text = "Logistics";
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
            tradeButtons[4].Texture = Content.Load<Texture2D>("ChocolateIcon");
        
            tradeButtons[5] = new UIButton();
            tradeButtons[5].OnClick += addItemToTrade;
            tradeButtons[5].Texture = Content.Load<Texture2D>("PhoneIcon");

            tradeButtons[6] = new UIButton();
            tradeButtons[6].OnClick += addItemToTrade;
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

        private void makeTrade(UIEventArg arg)
        {

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
            selectedNation = 1;
        }

        private void selectNation2(UIEventArg arg)
        {
            selectedNation = 2;
        }

        private void OnNextDayClick(UIEventArg arg)
        {
            //isTransitioning = true;
            //isUiActive = false;
            //state = GameTransitionState.FADE_IN;
            world.ProgressDay();
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

        /// <summary>
        /// Updates the instance fo the <c>GameScreen</c>.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
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
                        world.ProgressDay();
                        state = GameTransitionState.FADE_OUT;
                        break;
                    case GameTransitionState.FADE_IN:
                        opacity += FADE_INTERVAL;
                        if (opacity > 1)
                            state = GameTransitionState.DO_STUFF;
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

            if (InputManager.Instance.KeyPressed(Keys.Escape))
                form = UserInterfaceForms.GAMEWORLD;

        }

        /// <summary>
        /// Draws the contents of the <c>GameScreen</c>.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.PointWrap, null, null, null, null);
            waterMap.Draw(spriteBatch);

            world.Draw(spriteBatch);

            Rectangle formRectangle = new Rectangle(16, 16, game.Width - 32, game.Height - 32);

            if (isUiActive)
            {
                switch (form)
                {
                    case UserInterfaceForms.GAMEWORLD:
                        DrawGameWorld(formRectangle, spriteBatch);
                        break;

                    case UserInterfaceForms.LOGISTICS:

                        break;

                    case UserInterfaceForms.PRODUCTION:
                        DrawProduction(formRectangle, spriteBatch);
                        break;

                    case UserInterfaceForms.TRADE:
                        DrawTrade(formRectangle, spriteBatch);
                        break;
                }
            }

            spriteBatch.Draw(blank, new Rectangle(0, 0, game.Width, game.Height), Color.Black * opacity);
            spriteBatch.End();
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
                int sel = 0;
                if (!isYourInventory)
                {
                    sel = selectedNation;
                }
                // choc, ph, sh
                string text = "";
                if (i == 0)
                {
                    text = "Chocolate\nQuantity: " + (int)world.Nations[sel].Chocolates;
                }
                else if (i == 1)
                {
                    text = "Phone\nQuantity: " + (int)world.Nations[sel].Phones;
                }
                else if (i == 2)
                {
                    text = "Shirt\nQuantity: " + (int)world.Nations[sel].Shirts;
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
                        + height2 * (j - y * 5), fRect.Y + space * (y + 1) + height2 * y, height2, height2), Color.White);

                    num0++;
                }
                num1 = num0;
                for (int j = num0; j < (int)shirtAdd + num1; j++)
                {
                    int y = num0 / 5;

                    spriteBatch.Draw(goodIcons[4], new Rectangle(fRect2.X + spacing2 * (j + 1 - y * 5)
                        + height2 * (j - y * 5), fRect.Y + space * (y + 1) + height2 * y, height2, height2), Color.White);

                    num0++;
                }
                num1 = num0;
                for (int j = num0; j < (int)phoneAdd + num1; j++)
                {
                    int y = num0 / 5;

                    spriteBatch.Draw(goodIcons[5], new Rectangle(fRect2.X + spacing2 * (j + 1 - y * 5)
                        + height2 * (j - y * 5), fRect.Y + space * (y + 1) + height2 * y, height2, height2), Color.White);

                    num0++;
                }
            }

            tradeButtons[7].Position = new Point(fRect.X + fRect.Width / 2 - 180,
                fRect.Y + fRect.Height - 72);

            foreach (UIButton but in tradeButtons)
            {
                but.Draw(spriteBatch);
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
                formRectangle.Width / 4 - capitalPos.X / 2, formRectangle.Y + titlePos.Y * 2), Color.White);
            spriteBatch.DrawString(fontb, "Consumer", new Vector2(formRectangle.X + 3 *
                formRectangle.Width / 4 - consumerPos.X / 2, formRectangle.Y + titlePos.Y * 2), Color.White);

            int spacing = 72;
            int height = (activeRect.Height - 4 * spacing) / 3;
            int width = activeRect.Width / 2 - 124;
            int buttonDim = height - 16;
            for (int i = 0; i < 3; i++)
            {
                Rectangle rect0 = new Rectangle(activeRect.X + 16, activeRect.Y
                    + spacing * (i + 1) + i * height, width, height);
                Rectangle rect1 = new Rectangle(activeRect.X + activeRect.Width
                    - width - 16, activeRect.Y + spacing * (i + 1) + i * height, width, height);
                productionBtns[i].Size = new Point(buttonDim, buttonDim);
                productionBtns[i].Position = new Point(rect0.X + rect0.Width - buttonDim - 8, rect0.Y + 8);
                productionBtns[i + 3].Size = new Point(buttonDim, buttonDim);
                productionBtns[i + 3].Position = new Point(rect1.X + rect1.Width - 8 - buttonDim, rect1.Y + 8);
                productionBtns[i + 6].Size = new Point(buttonDim, buttonDim);
                productionBtns[i + 6].Position = new Point(rect0.X + rect0.Width - buttonDim * 2 - 16, rect0.Y + 8);
                productionBtns[i + 9].Size = new Point(buttonDim, buttonDim);
                productionBtns[i + 9].Position = new Point(rect1.X + rect1.Width - buttonDim * 2 - 16, rect1.Y + 8);
                spriteBatch.Draw(blank, rect0, Color.DarkGreen);
                spriteBatch.Draw(goodIcons[i], new Rectangle(rect0.X + 2, rect0.Y + 2, rect0.Height - 4, rect0.Height - 4), Color.White);
                spriteBatch.Draw(blank, rect1, Color.DarkGreen);
                spriteBatch.Draw(goodIcons[i + 3], new Rectangle(rect1.X + 2, rect1.Y + 2, rect0.Height - 4, rect0.Height - 4), Color.White);

                // itemname
                // quantity: #
                // queued: #
                string capitalText = "";
                if (i == 0)
                {
                    int percent = (int)((n.QueuedFactories - (int)n.QueuedFactories) * 100);
                    capitalText = "Factory\nAdvantage: " + n.FactoryAdvantage + "\nQuantity: " + (int)n.Tools + "\nQueued: " +
                        (int)Math.Ceiling(n.QueuedFactories) + "\nCompleted: " + percent + "%";
                }
                else if (i == 1)
                {
                    int percent = (int)((n.QueuedTrucks - (int)n.QueuedTrucks) * 100);
                    capitalText = "Truck\nAdvantage: " + n.TruckAdvantage + "\nQuantity: " + (int)n.Trucks + "\nQueued: " +
                        (int)Math.Ceiling(n.QueuedTrucks) + "\nCompleted: " + percent + "%";
                }
                else if (i == 2)
                {
                    int percent = (int)((n.QueuedTools - (int)n.QueuedTools) * 100);
                    capitalText = "Set of Tools\nAdvantage: " + n.ToolAdvantage + "\nQuantity: " + (int)n.Tools + "\nQueued: " +
                        (int)Math.Ceiling(n.QueuedTools) + "\nCompleted: " + percent + "%";
                }

                spriteBatch.DrawString(font, capitalText, new Vector2(rect0.X + height + 4, productionBtns[i].Position.Y), Color.White);

                string consumerText = "";
                if (i == 0)
                {
                    int percent = (int)((n.QueuedChocolates - (int)n.QueuedChocolates) * 100);
                    consumerText = "Chocolate\nAdvantage: " + n.ChocolateAdvantage + "\nQuantity: " + (int)n.Chocolates + "\nQueued: " +
                        (int)Math.Ceiling(n.QueuedChocolates) + "\nCompleted: " + percent + "%";
                }
                else if (i == 1)
                {
                    int percent = (int)((n.QueuedShirts - (int)n.QueuedShirts) * 100);
                    consumerText = "Shirt\nAdvantage: " + n.ShirtAdvantage+ "\nQuantity: " + (int)n.Shirts + "\nQueued: " +
                        (int)Math.Ceiling(n.QueuedShirts) + "\nCompleted: " + percent + "%";
                }
                else if (i == 2)
                {
                    int percent = (int)((n.QueuedPhones - (int)n.QueuedPhones) * 100);
                    consumerText = "Phone\nAdvantage: " + n.PhoneAdvantage + "\nQuantity: " + (int)n.Phones + "\nQueued: " +
                        (int)Math.Ceiling(n.QueuedPhones) + "\nCompleted: " + percent + "%";
                }

                spriteBatch.DrawString(font, consumerText, new Vector2(rect1.X + height + 4, 
                    productionBtns[i].Position.Y), Color.White);
            }

            foreach (UIButton button in productionBtns)
                button.Draw(spriteBatch);
            
        }

    }
}
