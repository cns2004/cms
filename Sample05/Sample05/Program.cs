using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Sample05
{
    class Program
    {
        static void Main(string[] args)
        {
            //test_insert();
            //test_mult_insert();
            //test_del();
            //test_mult_del();
            //test_select_one();
            //test_select_list();
            //test_mult_commetinsert();
            test_select_content_with_comment();
            Console.ReadLine();
        }

        /// <summary>
        /// 测试插入单条数据
        /// </summary>
        static void test_insert()
        {
            var content = new Content
            {
                title = "标题1",
                content = "内容1",

            };
            using (var conn = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=cms;Trusted_Connection=True;MultipleActiveResultSets=true;"))
            {
                string sql_insert = @"INSERT INTO [Content]
                (title, [content], status, add_time, modify_time)
VALUES   (@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_insert：插入了{result}条数据！");
            }
        }

        /// <summary>
        /// 测试一次批量插入两条数据
        /// </summary>
        static void test_mult_insert()
        {
            List<Content> contents = new List<Content>() {
               new Content
            {
                title = "批量插入标题1",
                content = "批量插入内容1",

            },
               new Content
            {
                title = "批量插入标题2",
                content = "批量插入内容2",

            },
                new Content
            {
                title = "批量插入标题3",
                content = "批量插入内容3",

            },
                 new Content
            {
                title = "批量插入标题4",
                content = "批量插入内容4",

            },
                  new Content
            {
                title = "批量插入标题5",
                content = "批量插入内容5",

            },
                   new Content
            {
                title = "批量插入标题6",
                content = "批量插入内容6",

            },
        };

            using (var conn = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=cms;Trusted_Connection=True;MultipleActiveResultSets=true;"))
            {
                string sql_insert = @"INSERT INTO [Content]
                (title, [content], status, add_time, modify_time)
VALUES   (@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_mult_insert：插入了{result}条数据！");
            }
        }
        /// <summary>
        /// 测试删除单条数据
        /// </summary>
        static void test_del()
        {
            var content = new Content
            {
                id = 1,

            };
            using (var conn = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=cms;Trusted_Connection=True;MultipleActiveResultSets=true;"))
            {
                string sql_insert = @"DELETE FROM [Content]
WHERE   (id = @id)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_del：删除了{result}条数据！");
            }
        }

        /// <summary>
        /// 测试一次批量删除两条数据
        /// </summary>
        static void test_mult_del()
        {
            List<Content> contents = new List<Content>() {
               new Content
            {
                id=2,

            },
               new Content
            {
                id=3,

            },
        };

            using (var conn = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=cms;Trusted_Connection=True;MultipleActiveResultSets=true;"))
            {
                string sql_insert = @"DELETE FROM [Content]
WHERE   (id = @id)";
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_mult_del：删除了{result}条数据！");
            }
        }
        /// <summary>
        /// 查询单条指定的数据
        /// </summary>
        static void test_select_one()
        {
            using (var conn = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=cms;Trusted_Connection=True;MultipleActiveResultSets=true;"))
            {
                string sql_insert = @"select * from [dbo].[content] where id=@id";
                var result = conn.QueryFirstOrDefault<Content>(sql_insert, new { id = 5 });
                Console.WriteLine($"test_select_one：查到的数据为：");
            }
        }

        /// <summary>
        /// 查询多条指定的数据
        /// </summary>
        static void test_select_list()
        {
            using (var conn = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=cms;Trusted_Connection=True;MultipleActiveResultSets=true;"))
            {
                string sql_insert = @"select * from [dbo].[content] where id in @ids";
                var result = conn.Query<Content>(sql_insert, new { ids = new int[] { 3,4,5 } });
                Console.WriteLine($"test_select_one：查到的数据为：");
            }
        }

        static void test_mult_commetinsert()
        {
            List<Comment> commets = new List<Comment>() {
               new Comment
            {
                content="没有意思",
                content_id=5,
            },
               new Comment
            {
                content="你这是认真的吗?",
                content_id=5,
            },
        };

            using (var conn = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=cms;Trusted_Connection=True;MultipleActiveResultSets=true;"))
            {
                string sql_insert = @"INSERT INTO [comment]
                (content_id,content,add_time)
VALUES   (@content_id,@content,@add_time)";
                var result = conn.Execute(sql_insert, commets);
                Console.WriteLine($"test_mult_insert：插入了{result}条评论！");
            }
        }
        static void test_select_content_with_comment()
        {
            using (var conn = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=cms;Trusted_Connection=True;MultipleActiveResultSets=true;"))
            {
                string sql_insert = @"select * from content where id=@id;
select * from comment where content_id=@id;";
                using (var result = conn.QueryMultiple(sql_insert, new { id = 5 }))
                {
                    var content = result.ReadFirstOrDefault<ContentWithCommnet>();
                    content.comments = result.Read<Comment>();
                    Console.WriteLine($"test_select_content_with_comment:内容5的评论数量{content.comments.Count()}");
                }

            }
        }
    }
    public class Content
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 状态 1正常 0删除
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime add_time { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? modify_time { get; set; }
    }
    public class Comment
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 文章id
        /// </summary>
        public int content_id { get; set; }
        /// <summary>
        /// 评论内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime add_time { get; set; } = DateTime.Now;
    }
    public class ContentWithCommnet
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 状态 1正常 0删除
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime add_time { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? modify_time { get; set; }
        /// <summary>
        /// 文章评论
        /// </summary>
        public IEnumerable<Comment> comments { get; set; }
    }
}
