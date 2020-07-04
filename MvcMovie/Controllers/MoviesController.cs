//#define USELINQ
//#define USEMYSQL
#define USEADONET

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using MySql.Data.MySqlClient;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;
        // DBの接続文字列
        private const string _connstr = "Server=localhost;Database=moviedb;User=root;Password=p@ssw0rd";

        public MoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
#if USELINQ
            // LINQを使用した場合
            return View(await _context.Movie.ToListAsync());
#endif
#if USEMYSQL
            // MySQLオブジェクトを使用した場合
            List<Movie> movies = new List<Movie>();             // 取得データのリスト
            MySqlConnection conn = null;                                // MySQL接続オブジェクト
            try
            {
                conn = new MySqlConnection(_connstr);           // 接続文字列の設定
                await conn.OpenAsync();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT");
                sql.Append(" ID");
                sql.Append(",Genre");
                sql.Append(",Price");
                sql.Append(",ReleaseDate");
                sql.Append(",Title");
                sql.Append(" FROM");
                sql.Append(" trn_movie");

                using (MySqlCommand cmd = new MySqlCommand(sql.ToString(), conn))
                {
                    DbDataReader reader = await cmd.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Movie
                            {
                                Id = reader.GetInt32(0),
                                Genre = reader.GetString(1),
                                Price = reader.GetDecimal(2),
                                ReleaseDate = reader.GetDateTime(3),
                                Title = reader.GetString(4),
                            };
                            movies.Add(row);
                        }
                    }
                    await reader.DisposeAsync();
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return View(movies);
#endif
#if USEADONET
            // ADO.NETを使用した場合
            List<Movie> movies = new List<Movie>();                     // Listオブジェクト

            // コンテキストから接続情報を取得
            var conn = _context.Database.GetDbConnection();

            try
            {
                // 接続をオープン
                await conn.OpenAsync();

                // Commandオブジェクトのライフタイムを制限
                using (var cmd = conn.CreateCommand())
                {
                    // SQL文字列を作成
                    StringBuilder sql = new StringBuilder();
                    sql.Append("SELECT");
                    sql.Append("   trn_movie.id_movie");
                    sql.Append("  ,mst_genre.nm_genre");
                    sql.Append("  ,trn_movie.kin_price");
                    sql.Append("  ,trn_movie.dt_release");
                    sql.Append("  ,trn_movie.nm_title");
                    sql.Append(" FROM");
                    sql.Append("    trn_movie");
                    sql.Append(" INNER JOIN");
                    sql.Append("    mst_genre");
                    sql.Append(" ON");
                    sql.Append("    trn_movie.kbn_genre=mst_genre.id_genre");
                    // CommandオブジェクトにSQLをセット
                    cmd.CommandText = sql.ToString();
                    // SQLを実行
                    DbDataReader reader = await cmd.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        // 取得したレコード分だけループする
                        while (await reader.ReadAsync())
                        {
                            // Movieオブジェクトに取得結果をセットする
                            var row = new Movie
                            {
                                Id = reader.GetInt32(0),                            // ID
                                Genre = reader.GetString(1),                    // ジャンル
                                Price = reader.GetDecimal(2),                   // 価格
                                ReleaseDate = reader.GetDateTime(3),    // リリース日
                                Title = reader.GetString(4),                        // タイトル
                            };
                            // Listオブジェクトに追加
                            movies.Add(row);
                        }
                    }
                    // Readerオブジェクトを破棄
                    await reader.DisposeAsync();
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
            }
            finally
            {
                // DB接続をクローズ
                conn.Close();
            }

            return View(movies);
#endif
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

#if USELINQ
            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
#endif
#if USEADONET
            Movie movie = new Movie();

            // コンテキストから接続情報を取得
            var conn = _context.Database.GetDbConnection();

            try
            {
                // 接続をオープン
                await conn.OpenAsync();

                // Commandオブジェクトのライフタイムを制限
                using (var cmd = conn.CreateCommand())
                {
                    // SQL文字列を作成
                    StringBuilder sql = new StringBuilder();
                    sql.Append("SELECT");
                    sql.Append("   trn_movie.id_movie");
                    sql.Append("  ,mst_genre.nm_genre");
                    sql.Append("  ,trn_movie.kin_price");
                    sql.Append("  ,trn_movie.dt_release");
                    sql.Append("  ,trn_movie.nm_title");
                    sql.Append(" FROM");
                    sql.Append("    trn_movie");
                    sql.Append(" INNER JOIN");
                    sql.Append("    mst_genre");
                    sql.Append(" ON");
                    sql.Append("    trn_movie.kbn_genre=mst_genre.id_genre");
                    sql.Append(" WHERE");
                    sql.Append("    trn_movie.id_movie=@ID");
                    // CommandオブジェクトにSQLをセット
                    cmd.CommandText = sql.ToString();
                    cmd.Parameters.Add(new MySqlParameter("ID", id));
                    // SQLを実行
                    DbDataReader reader = await cmd.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        // 取得したレコード分だけループする
                        while (await reader.ReadAsync())
                        {
                            // Movieオブジェクトに取得結果をセットする
                            movie.Id = reader.GetInt32(0);                            // ID
                            movie.Genre = reader.GetString(1);                    // ジャンル
                            movie.Price = reader.GetDecimal(2);                   // 価格
                            movie.ReleaseDate = reader.GetDateTime(3);    // リリース日
                            movie.Title = reader.GetString(4);                        // タイトル
                            break;
                        }
                    }
                    // Readerオブジェクトを破棄
                    await reader.DisposeAsync();
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
            }
            finally
            {
                // DB接続をクローズ
                conn.Close();
            }

            return View(movie);
#endif
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
