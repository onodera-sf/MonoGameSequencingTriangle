using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SequencingTriangle
{
	/// <summary>
	/// ゲームメインクラス
	/// </summary>
	public class Game1 : Game
	{
    /// <summary>
    /// グラフィックデバイス管理クラス
    /// </summary>
    private readonly GraphicsDeviceManager _graphics = null;

    /// <summary>
    /// スプライトのバッチ化クラス
    /// </summary>
    private SpriteBatch _spriteBatch = null;

    /// <summary>
    /// TriangleList 用頂点バッファ
    /// </summary>
    private VertexBuffer _triangleListVertexBuffer = null;

    /// <summary>
    /// TriangleStrip 用頂点バッファ
    /// </summary>
    private VertexBuffer _triangleStripVertexBuffer = null;

    /// <summary>
    /// 基本エフェクト
    /// </summary>
    private BasicEffect _basicEffect = null;


    /// <summary>
    /// GameMain コンストラクタ
    /// </summary>
    public Game1()
    {
      // グラフィックデバイス管理クラスの作成
      _graphics = new GraphicsDeviceManager(this);

      // ゲームコンテンツのルートディレクトリを設定
      Content.RootDirectory = "Content";

      // マウスカーソルを表示
      IsMouseVisible = true;
    }

    /// <summary>
    /// ゲームが始まる前の初期化処理を行うメソッド
    /// グラフィック以外のデータの読み込み、コンポーネントの初期化を行う
    /// </summary>
    protected override void Initialize()
    {
      // TODO: ここに初期化ロジックを書いてください

      // コンポーネントの初期化などを行います
      base.Initialize();
    }

    /// <summary>
    /// ゲームが始まるときに一回だけ呼ばれ
    /// すべてのゲームコンテンツを読み込みます
    /// </summary>
    protected override void LoadContent()
    {
      // テクスチャーを描画するためのスプライトバッチクラスを作成します
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      // エフェクトを作成
      _basicEffect = new BasicEffect(GraphicsDevice);

      // エフェクトで頂点カラーを有効にする
      _basicEffect.VertexColorEnabled = true;

      // ビューマトリックスをあらかじめ設定 ((0, 0, 12) から原点を見る)
      _basicEffect.View = Matrix.CreateLookAt(
              new Vector3(0.0f, 0.0f, 12.0f),
              Vector3.Zero,
              Vector3.Up
          );

      // プロジェクションマトリックスをあらかじめ設定
      _basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(
              MathHelper.ToRadians(45.0f),
              (float)GraphicsDevice.Viewport.Width /
                  (float)GraphicsDevice.Viewport.Height,
              1.0f,
              100.0f
          );

      // TriangleList 用頂点バッファ作成
      _triangleListVertexBuffer = new VertexBuffer(GraphicsDevice,
          VertexPositionColor.VertexDeclaration, 6, BufferUsage.None);

      // TriangleList 用頂点データを作成する
      VertexPositionColor[] triangleListVertices = new VertexPositionColor[6];

      triangleListVertices[0] = new VertexPositionColor(new Vector3(-2.5f, 0.5f, 0.0f),
                                                        Color.Blue);
      triangleListVertices[1] = new VertexPositionColor(new Vector3(-1.5f, 2.5f, 0.0f),
                                                        Color.Green);
      triangleListVertices[2] = new VertexPositionColor(new Vector3(-0.5f, 0.5f, 0.0f),
                                                        Color.Red);
      triangleListVertices[3] = new VertexPositionColor(new Vector3(0.5f, 0.5f, 0.0f),
                                                        Color.Yellow);
      triangleListVertices[4] = new VertexPositionColor(new Vector3(1.5f, 2.5f, 0.0f),
                                                        Color.Black);
      triangleListVertices[5] = new VertexPositionColor(new Vector3(2.5f, 0.5f, 0.0f),
                                                        Color.White);

      // 頂点データを頂点バッファに書き込む
      _triangleListVertexBuffer.SetData(triangleListVertices);

      // TriangleStrip 用頂点バッファ作成
      _triangleStripVertexBuffer = new VertexBuffer(GraphicsDevice,
          VertexPositionColor.VertexDeclaration, 6, BufferUsage.None);

      // TriangleStrip 用頂点データを作成する
      VertexPositionColor[] triangleStripVertices = new VertexPositionColor[6];

      triangleStripVertices[0] = new VertexPositionColor(new Vector3(-2.5f, -2.5f, 0.0f),
                                                         Color.Red);
      triangleStripVertices[1] = new VertexPositionColor(new Vector3(-1.5f, -0.5f, 0.0f),
                                                         Color.Blue);
      triangleStripVertices[2] = new VertexPositionColor(new Vector3(-0.5f, -2.5f, 0.0f),
                                                         Color.Green);
      triangleStripVertices[3] = new VertexPositionColor(new Vector3(0.5f, -0.5f, 0.0f),
                                                         Color.Yellow);
      triangleStripVertices[4] = new VertexPositionColor(new Vector3(1.5f, -2.5f, 0.0f),
                                                         Color.Black);
      triangleStripVertices[5] = new VertexPositionColor(new Vector3(2.5f, -0.5f, 0.0f),
                                                         Color.White);

      // 頂点データを頂点バッファに書き込む
      _triangleStripVertexBuffer.SetData(triangleStripVertices);
    }

    /// <summary>
    /// ゲームが終了するときに一回だけ呼ばれ
    /// すべてのゲームコンテンツをアンロードします
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: ContentManager で管理されていないコンテンツを
      //       ここでアンロードしてください
    }

    /// <summary>
    /// 描画以外のデータ更新等の処理を行うメソッド
    /// 主に入力処理、衝突判定などの物理計算、オーディオの再生など
    /// </summary>
    /// <param name="gameTime">このメソッドが呼ばれたときのゲーム時間</param>
    protected override void Update(GameTime gameTime)
    {
      // ゲームパッドの Back ボタン、またはキーボードの Esc キーを押したときにゲームを終了させます
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
      {
        Exit();
      }

      // TODO: ここに更新処理を記述してください

      // 登録された GameComponent を更新する
      base.Update(gameTime);
    }

    /// <summary>
    /// 描画処理を行うメソッド
    /// </summary>
    /// <param name="gameTime">このメソッドが呼ばれたときのゲーム時間</param>
    protected override void Draw(GameTime gameTime)
    {
      // 画面を指定した色でクリアします
      GraphicsDevice.Clear(Color.CornflowerBlue);

      // 描画に使用する頂点バッファをセットします
      GraphicsDevice.SetVertexBuffer(_triangleListVertexBuffer);

      // パスの数だけ繰り替えし描画
      foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
      {
        // パスの開始
        pass.Apply();

        // TriangleList で描画する
        GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
      }

      // 描画に使用する頂点バッファをセットします
      GraphicsDevice.SetVertexBuffer(_triangleStripVertexBuffer);

      // パスの数だけ繰り替えし描画
      foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
      {
        // パスの開始
        pass.Apply();

        // TriangleStrip で描画する
        GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 4);
      }

      // 登録された DrawableGameComponent を描画する
      base.Draw(gameTime);
    }
  }
}
