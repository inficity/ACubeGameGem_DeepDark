
using DeepDark.Server;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace DeepDark
{
	public class CardManager
	{
		private static CardManager Instance;

		private Dictionary<int, Card> Cards = new Dictionary<int, Card>();

		public CardManager()
		{
			{
				var card = new Card();
				card.Id = 101;
				card.Name = "직장 선배";
				card.Description = "45세 / 노처녀, 매일 술을 마시며, 히스테리를 부린다.";
				card.Cost = 3;
				card.Power = 2;
				card.HP = 2;
				card.IsNegative = true;
				card.OnUseCard = (me, op) =>
				{
					op.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) =>
				{
					serverChar.setAttackChance(1);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 102;
				card.Name = "꼰대 대리";
				card.Description = "40세 / 쌍둥이 아빠, 야근에 찌들어 산다.";
				card.Cost = 3;
				card.Power = 3;
				card.HP = 1;
				card.IsNegative = true;
				card.OnUseCard = (me, op) =>
				{
					op.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) =>
				{
					serverChar.setAttackChance(1);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 103;
				card.Name = "만년 과장";
				card.Description = "50세 / 매일 지각을 하며, 욕만 먹고 산다. 부하직원을 홀대한다.";
				card.Cost = 4;
				card.Power = 1;
				card.HP = 5;
				card.IsNegative = true;
				card.OnUseCard = (me, op) =>
				{
					op.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) =>
				{
					serverChar.setAttackChance(1);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 104;
				card.Name = "부장";
				card.Description = "46세 / 과속 승진, 만년 과장에게 항상 화를 내고 있음.";
				card.Cost = 7;
				card.Power = 5;
				card.HP = 6;
				card.IsNegative = true;
				card.OnUseCard = (me, op) =>
				{
					op.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) =>
				{
					serverChar.setAttackChance(1);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 105;
				card.Name = "이사";
				card.Description = "55세 / 하는 일도 없고 매번 여자 직원에게 추파를 던진다.";
				card.Cost = 10;
				card.Power = 7;
				card.HP = 3;
				card.IsNegative = true;
				card.OnUseCard = (me, op) =>
				{
					op.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) =>
				{
					serverChar.setAttackChance(2);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 106;
				card.Name = "사장";
				card.Description = "60세 / 골프가 취미, 돈만 밝히며, 항상 놀 궁리만 하고 있다.";
				card.Cost = 15;
				card.Power = 13;
				card.HP = 10;
				card.IsNegative = true;
				card.OnUseCard = (me, op) =>
				{
					op.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) =>
				{
					serverChar.setAttackChance(1);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 107;
				card.Name = "회장";
				card.Description = "70세 / 병자 코스프레 말기 환자.";
				card.Cost = 20;
				card.Power = 15;
				card.HP = 6;
				card.IsNegative = true;
				card.OnUseCard = (me, op) =>
				{
					op.spawnCard(card.Id);
					op.spawnCard(101);
					op.spawnCard(101);
				};
				card.OnBeginTurn = (serverChar, me, op) =>
				{
					serverChar.setAttackChance(1);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 201;
				card.Name = "저주받은 수녀";
				card.Description = "억울한 죽음을 당한 수녀. [공포]";
				card.Cost = 3;
				card.Power = 0;
				card.HP = 0;
				card.IsNegative = true;
				card.OnUseCard = (me, op) =>
				{
					if (me.haveTag("shield"))
						return;

					var buff = new Buff("공포", 1);
					buff.addTag("freeze");

					me.addBuff(buff);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 202;
				card.Name = "야근";
				card.Description = "오늘도 새벽까지 야근이다……";
				card.Cost = 2;
				card.Power = 0;
				card.HP = 0;
				card.IsNegative = true;
				card.OnUseCard = (me, op) =>
				{
					if (me.haveTag("shield"))
						return;

					me.addHP(-3);
					me.sendChangedMessage();
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 203;
				card.Name = "임금 체불";
				card.Description = "내돈은 어디로… 벌써 3달 째다.";
				card.Cost = 4;
				card.Power = 0;
				card.HP = 0;
				card.IsNegative = true;
				card.OnUseCard = (me, op) =>
				{
					if (me.haveTag("shield"))
						return;

					me.addHP(-6);
					me.sendChangedMessage();
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 204;
				card.Name = "알 수 없는 버그";
				card.Description = "게임을 만들었는데 왜 안되는거니? 너란 버그 어디있는거니?";
				card.Cost = 4;
				card.Power = 0;
				card.HP = 0;
				card.IsNegative = true;
				card.OnUseCard = (me, op) =>
				{
					if (me.haveTag("shield"))
						return;

					var buff = new Buff("버그", 5);
					buff.onBeginTurn((me1, op1) =>
					{
						me1.addHP(-1);
						me1.sendChangedMessage();
					});

					me.addBuff(buff);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 205;
				card.Name = "갑질 고객";
				card.Description = "세상에 또라이는 많고, 그 또라이들은 다 나와 엮인다.";
				card.Cost = 5;
				card.Power = 0;
				card.HP = 0;
				card.IsNegative = true;
				card.OnUseCard = (me, op) =>
				{
					if (me.haveTag("shield"))
						return;

					if (Random.Range(0, 2) == 1)
					{
						me.addHP(-5);
						me.sendChangedMessage();
					}

					var buff = new Buff("도박", 2);
					buff.addTag("aggro");

					me.addBuff(buff);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 206;
				card.Name = "상사의 갈굼";
				card.Description = "눼, 즤승합늬다……!";
				card.Cost = 5;
				card.Power = 0;
				card.HP = 0;
				card.IsNegative = true;
				card.OnUseCard = (me, op) =>
				{
					if (me.haveTag("shield"))
						return;

					var buff = new Buff("울분", 1);
					buff.addTag("+30%");

					me.addBuff(buff);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 207;
				card.Name = "권고 사직";
				card.Description = "XX씨, 자네가 일을 잘하는거 아는데, 회사가 어려워서…";
				card.Cost = 7;
				card.Power = 0;
				card.HP = 0;
				card.IsNegative = true;
				card.OnUseCard = (me, op) =>
				{
					if (me.haveTag("shield"))
						return;

					me.addCost(-10);
					me.sendChangedMessage();

					var buff = new Buff("마음의 상처", 2);
					buff.onBeginTurn((me1, op1) =>
					{
						me1.addHP(-5);
						me1.sendChangedMessage();
					});

					me.addBuff(buff);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 301;
				card.Name = "여자 친구";
				card.Description = "25세 / 예쁘다, 사랑스럽다, 섹시하다.";
				card.Cost = 5;
				card.Power = 3;
				card.HP = 3;
				card.IsNegative = false;
				card.OnUseCard = (me, op) =>
				{
					me.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) =>
				{
					serverChar.setAttackChance(1);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 302;
				card.Name = "소꿉 친구";
				card.Description = "27세 / 20년째 친구, 백수.";
				card.Cost = 5;
				card.Power = 4;
				card.HP = 1;
				card.IsNegative = false;
				card.OnUseCard = (me, op) =>
				{
					me.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) =>
				{
					serverChar.setAttackChance(1);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 303;
				card.Name = "어머니";
				card.Description = "53세 / 짜장면을 싫어하심, 김치를 잘 담그심.";
				card.Cost = 7;
				card.Power = 7;
				card.HP = 4;
				card.IsNegative = false;
				card.OnUseCard = (me, op) =>
				{
					me.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) =>
				{
					serverChar.setAttackChance(1);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 304;
				card.Name = "아버지";
				card.Description = "55세 / 호랑이 아버지이지만 항상 우리를 생각하신다.";
				card.Cost = 7;
				card.Power = 4;
				card.HP = 7;
				card.IsNegative = false;
				card.OnUseCard = (me, op) =>
				{
					me.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) =>
				{
					serverChar.setAttackChance(1);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 305;
				card.Name = "강아지(웰시코기)";
				card.Description = "2살 / 커엽다, 빵댕이, 하악….♥";
				card.Cost = 10;
				card.Power = 5;
				card.HP = 5;
				card.IsNegative = false;
				card.OnUseCard = (me, op) =>
				{
					me.spawnCard(card.Id).setAttackChance(2);
				};
				card.OnBeginTurn = (serverChar, me, op) =>
				{
					serverChar.setAttackChance(2);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 306;
				card.Name = "고양이(검은고양이)";
				card.Description = "1살 / 뽀시래기, 귀엽지 않았다면 벌써 죽었다.";
				card.Cost = 10;
				card.Power = 8;
				card.HP = 3;
				card.IsNegative = false;
				card.OnUseCard = (me, op) =>
				{
					me.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) =>
				{
					serverChar.setAttackChance(1);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 307;
				card.Name = "최애캐";
				card.Description = "언제나 너와 함께야 ~ XX찡……!";
				card.Cost = 30;
				card.Power = 15;
				card.HP = 15;
				card.IsNegative = false;
				card.OnUseCard = (me, op) =>
				{
					me.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) =>
				{
					serverChar.setAttackChance(1);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 401;
				card.Name = "여행";
				card.Description = "푸르른 바다, 찬란한 햇살, 아, 좋다.";
				card.Cost = 7;
				card.Power = 0;
				card.HP = 0;
				card.IsNegative = false;
				card.OnUseCard = (me, op) =>
				{
					me.addHP(8);
					me.sendChangedMessage();
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 402;
				card.Name = "게임";
				card.Description = "푸슝 푸슝 푸시~융~";
				card.Cost = 8;
				card.Power = 0;
				card.HP = 0;
				card.IsNegative = false;
				card.OnUseCard = (me, op) =>
				{
					var buff = new Buff("방구석 워리어", 2);
					buff.addTag("invincible");

					me.addBuff(buff);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 403;
				card.Name = "데이트";
				card.Description = "핑크 빛 찬란한 나날이다.";
				card.Cost = 8;
				card.Power = 0;
				card.HP = 0;
				card.IsNegative = false;
				card.OnUseCard = (me, op) =>
				{
					var buff = new Buff("핑크", 3);
					buff.onBeginTurn((me1, op1) =>
					{
						me1.addHP(2);
						me1.sendChangedMessage();
					});

					me.addBuff(buff);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 404;
				card.Name = "음악";
				card.Description = "Music Is My Life";
				card.Cost = 10;
				card.Power = 0;
				card.HP = 0;
				card.IsNegative = false;
				card.OnUseCard = (me, op) =>
				{
					var buff = new Buff("볼륨", 2);
					buff.addTag("shield");

					me.addBuff(buff);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 405;
				card.Name = "보너스";
				card.Description = "월급 X2";
				card.Cost = 12;
				card.Power = 0;
				card.HP = 0;
				card.IsNegative = false;
				card.OnUseCard = (me, op) =>
				{
					var buff = new Buff("풍요", 2);
					buff.onBeginTurn((me1, op1) =>
					{
						me1.addHP(5);
						me1.sendChangedMessage();
					});

					me.addBuff(buff);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 406;
				card.Name = "월급입금확인";
				card.Description = "오늘은 금요일! 불금이다!!";
				card.Cost = 20;
				card.Power = 0;
				card.HP = 0;
				card.IsNegative = false;
				card.OnUseCard = (me, op) =>
				{
					var buff = new Buff("불금", 3);
					buff.addTag("+2");

					me.addBuff(buff);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 407;
				card.Name = "덕질";
				card.Description = "아… 미쿠짱… 하악";
				card.Cost = 15;
				card.Power = 0;
				card.HP = 0;
				card.IsNegative = false;
				card.OnUseCard = (me, op) =>
				{
					List<ServerCharacter> list = new List<ServerCharacter>();

					for (int i = 0; i < 2 && op.Field.Count > 0; ++i)
						list.Add(op.destroyServerCharacterRandom());

					foreach (var serverCharacter in list)
						me.spawnCard(serverCharacter);
				};
				Cards.Add(card.Id, card);
			}
		}

		static public Card GetCard(int id)
		{
			Init();
			Card value;
			if (Instance.Cards.TryGetValue(id, out value))
				return value;
			return null;
		}

		static public List<Card> GetAllPositiveCards()
		{
			Init();
			return Instance.Cards.Values.Where(c => !c.IsNegative)
				.ToList();
		}

		static public List<Card> GetAllNegativeCards()
		{
			Init();
			return Instance.Cards.Values.Where(c => c.IsNegative)
				.ToList();
		}

		static void Init()
		{
			if (Instance == null)
				Instance = new CardManager();
		}
	}
}
