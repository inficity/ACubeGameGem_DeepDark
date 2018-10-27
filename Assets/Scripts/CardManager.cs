
using System.Collections.Generic;
using System.Linq;

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
				card.OnUseCard = (me, op) => {
					op.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) => {
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
				card.OnUseCard = (me, op) => {
					op.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) => {
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
				card.OnUseCard = (me, op) => {
					op.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) => {
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
				card.OnUseCard = (me, op) => {
					op.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) => {
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
				card.OnUseCard = (me, op) => {
					op.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) => {
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
				card.OnUseCard = (me, op) => {
					op.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) => {
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
				card.OnUseCard = (me, op) => {
					op.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) => {
					serverChar.setAttackChance(1);
				};
				Cards.Add(card.Id, card);
			}


			{
				var card = new Card();
				card.Id = 202;
				card.Name = "야근";
				card.Description = "오늘도 새벽까지 야근이다...\n나에게 데미지3";
				card.Cost = 5;
				card.IsNegative = true;
				card.OnUseCard = (me, op) => {
					//me.damage(3);
				};
				Cards.Add(card.Id, card);
			}
			{
				var card = new Card();
				card.Id = 203;
				card.Name = "임금체불";
				card.Description = "내돈은 어디로... 벌써 3달째다\n나에게 데미지6";
				card.Cost = 5;
				card.IsNegative = true;
				card.OnUseCard = (me, op) => {
					//me.damage(6);
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
				card.OnUseCard = (me, op) => {
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
				card.OnUseCard = (me, op) => {
					me.addHP(-6);
					me.sendChangedMessage();
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
				card.OnUseCard = (me, op) => {
					me.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) => {
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
				card.OnUseCard = (me, op) => {
					me.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) => {
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
				card.OnUseCard = (me, op) => {
					me.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) => {
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
				card.OnUseCard = (me, op) => {
					me.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) => {
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
				card.OnUseCard = (me, op) => {
					me.spawnCard(card.Id).setAttackChance(2);
				};
				card.OnBeginTurn = (serverChar, me, op) => {
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
				card.OnUseCard = (me, op) => {
					me.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) => {
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
				card.OnUseCard = (me, op) => {
					me.spawnCard(card.Id);
				};
				card.OnBeginTurn = (serverChar, me, op) => {
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
				card.OnUseCard = (me, op) => {
					me.addHP(8);
					me.sendChangedMessage();
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

		static public List<Card> GetAllPositiveCards() {
			Init();
			return Instance.Cards.Values.Where(c => !c.IsNegative)
				.ToList();
		}
		
		static public List<Card> GetAllNegativeCards() {
			Init();
			return Instance.Cards.Values.Where(c => c.IsNegative)
				.ToList();
		}

		static void Init()
		{
			if (Instance == null)
			{
				Instance = new CardManager();
			}
		}
	}
}
