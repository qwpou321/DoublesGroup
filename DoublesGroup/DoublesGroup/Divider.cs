using System;
using System.Collections.Generic;
using System.Linq;

namespace DoublesGroup
{
    class Divider
    {
        // record games each player has played
        Dictionary<Player, int> m_gamesPlayerHasPlayed;
        List<Player> m_players;
        int m_maxLevelGapBetweenPlayers = 3;
        int m_nMinGamesPlayerHasPlayed;

        public List<Schedule> DividePlayers(List<Player> players, List<Schedule> scheduleHasPlayed)
        {
            InitData(players, scheduleHasPlayed);

            List<Player> playerOrder = new List<Player>();

            while (isFinish() == false && m_nMinGamesPlayerHasPlayed < 6)
            {
                List<Player> PlayersToBeGrouped = new List<Player>();
                List<Player> PlayersToBeGrouped2 = new List<Player>();
                foreach (KeyValuePair<Player, int> item in m_gamesPlayerHasPlayed)
                {
                    if (item.Value == m_nMinGamesPlayerHasPlayed) PlayersToBeGrouped.Add(item.Key);
                    if (item.Value == m_nMinGamesPlayerHasPlayed + 1) PlayersToBeGrouped2.Add(item.Key);
                }

                if (PlayersToBeGrouped.Count > 4)
                {
                    playerOrder.AddRange(GetGroup(new List<Player>(), PlayersToBeGrouped));
                    continue;
                }

                playerOrder.AddRange(GetGroup(PlayersToBeGrouped, PlayersToBeGrouped2));
                m_nMinGamesPlayerHasPlayed++;
            }

            List<string> scheduleOrder = new List<string>();

            for (int i = 0; i < playerOrder.Count; i += 4)
            {
                string set = playerOrder[i].Name + " , " + playerOrder[i + 1].Name + "  vs  " + playerOrder[i + 2].Name + " , " + playerOrder[i + 3].Name;
                scheduleOrder.Add(set);
            }

            List<Schedule> newAllSchedules = scheduleHasPlayed;

            for (int i = 0; i < scheduleOrder.Count; i++)
            {
                Schedule schedule = new Schedule();
                schedule.Players = scheduleOrder[i];
                newAllSchedules.Add(schedule);
            }

            return newAllSchedules;
        }

        void InitData(List<Player> players, List<Schedule> scheduleHasPlayed)
        {
            m_players = players;

            m_gamesPlayerHasPlayed = new Dictionary<Player, int>();
            foreach (Player player in players)
            {
                m_gamesPlayerHasPlayed.Add(player, 0);
            }

            if (scheduleHasPlayed.Count == 0)
            {
                m_nMinGamesPlayerHasPlayed = 0;
                return;
            }

            List<string> szPlayerList = new List<string>();
            foreach (Schedule schedule in scheduleHasPlayed)
            {
                string[] temp = schedule.Players.Split(new string[] { " , ", "  vs  " }, StringSplitOptions.RemoveEmptyEntries);
                szPlayerList.AddRange(temp);
            }

            int nMaxGamesPlayerHasPlayed = 0;

            foreach (Player player in players)
            {
                foreach (string temp in szPlayerList)
                {
                    if (player.Name == temp)
                    {
                        m_gamesPlayerHasPlayed[player]++;
                    }
                }

                if (m_gamesPlayerHasPlayed[player] > nMaxGamesPlayerHasPlayed)
                {
                    nMaxGamesPlayerHasPlayed++;
                }
            }

            m_nMinGamesPlayerHasPlayed = nMaxGamesPlayerHasPlayed - 1;

            foreach (Player player in players)
            {
                if (m_gamesPlayerHasPlayed[player] == 0)
                {
                    m_gamesPlayerHasPlayed[player] = m_nMinGamesPlayerHasPlayed;
                }
            }
        }

        List<Player> GetGroup(List<Player> chosenPlayers, List<Player> playersToBeGrouped)
        {
            Player firstPlayer;
            if (chosenPlayers.Count > 0) firstPlayer = chosenPlayers[0];
            else
            {
                firstPlayer = Get1stPlayer(playersToBeGrouped);
                chosenPlayers.Add(firstPlayer);
                playersToBeGrouped.Remove(firstPlayer);
            }

            Player secondPlayer;
            if (chosenPlayers.Count > 1) secondPlayer = chosenPlayers[1];
            else
            {
                secondPlayer = Get2ndOr3rdPlayer(firstPlayer, playersToBeGrouped);
                chosenPlayers.Add(secondPlayer);
                playersToBeGrouped.Remove(secondPlayer);
            }

            Player thirdPlayer;
            if (chosenPlayers.Count > 2) thirdPlayer = chosenPlayers[2];
            else
            {
                thirdPlayer = Get2ndOr3rdPlayer(firstPlayer, playersToBeGrouped);
                chosenPlayers.Add(thirdPlayer);
                playersToBeGrouped.Remove(thirdPlayer);
            }

            Player fourthPlayer;
            if (chosenPlayers.Count > 3) fourthPlayer = chosenPlayers[3];
            else
            {
                fourthPlayer = Get4thPlayer(chosenPlayers, playersToBeGrouped);
                chosenPlayers.Add(fourthPlayer);
            }

            foreach (Player player in chosenPlayers)
            {
                m_gamesPlayerHasPlayed[player] += 1;
            }

            ArrangeOrder(chosenPlayers);

            return chosenPlayers;
        }

        void ArrangeOrder(List<Player> chosenPlayers)
        {
            if (chosenPlayers.Count != 4) return;
            Player strongestPlayer = chosenPlayers[0];
            Player weakestPlayer = chosenPlayers[1];
            foreach (Player player in chosenPlayers)
            {
                if (player.Level > strongestPlayer.Level) strongestPlayer = player;
                if (player.Level < weakestPlayer.Level) weakestPlayer = player;
            }
            chosenPlayers.Remove(strongestPlayer);
            chosenPlayers.Remove(weakestPlayer);
            chosenPlayers.Add(strongestPlayer);
            chosenPlayers.Add(weakestPlayer);
        }

        Player Get1stPlayer(List<Player> playersToBeGrouped)
        {
            int numberOfPlayer = playersToBeGrouped.Count;
            Random random = new Random();
            return playersToBeGrouped[random.Next(numberOfPlayer)];
        }

        Player Get2ndOr3rdPlayer(Player firstPlayer, List<Player> playersToBeGrouped)
        {
            int numberOfPlayer = playersToBeGrouped.Count;
            if (numberOfPlayer < 1) return playersToBeGrouped[0];
            Random random = new Random();

            for (int i = 1; i < m_maxLevelGapBetweenPlayers; i++)
            {
                var suitablePlayers = from player in playersToBeGrouped
                                      where player.Level <= firstPlayer.Level + i &
                                            player.Level >= firstPlayer.Level - i
                                      select player;
                List<Player> players = suitablePlayers.ToList();
                if (players.Count > 0) return players[random.Next(players.Count)];
            }
            return playersToBeGrouped[random.Next(numberOfPlayer)];
        }

        Player Get4thPlayer(List<Player> chosendPlayers, List<Player> playersToBeGrouped)
        {
            int numberOfPlayer = playersToBeGrouped.Count;
            if (numberOfPlayer < 1) return playersToBeGrouped[0];

            // To find best and worst player in ChoosedPlayers
            int Best = 0, Worst = 1;
            for (int i = 0; i < 3; i++)
            {
                if (chosendPlayers[i].Level > chosendPlayers[Best].Level) Best = i;
                if (chosendPlayers[i].Level < chosendPlayers[Worst].Level) Worst = i;
            }
            // best index + worst index + middle index = sum of three player's index = 3 
            int Middle = 3 - Best - Worst;

            int BestLevel = chosendPlayers[Best].Level;
            int MiddleLevel = chosendPlayers[Middle].Level;
            int WorstLevel = chosendPlayers[Worst].Level;

            Random random = new Random();

            for (int i = 0; i < m_maxLevelGapBetweenPlayers; i++)
            {
                var suitablePlayers = from player in playersToBeGrouped
                                      where player.Level + MiddleLevel + i == WorstLevel + BestLevel
                                      select player;
                List<Player> players = suitablePlayers.ToList();
                if (players.Count > 0) return players[random.Next(players.Count)];
            }
            return playersToBeGrouped[random.Next(numberOfPlayer)];
        }

        // Everyone has to play the same number of games.
        bool isFinish()
        {
            // 0 means someone did not play the game.
            if (m_gamesPlayerHasPlayed[m_players[0]] == 0) return false;

            for (int i = 1; i < m_players.Count; i++)
            {
                if (m_gamesPlayerHasPlayed[m_players[i]] != m_gamesPlayerHasPlayed[m_players[0]]) return false;
            }
            return true;
        }
    }
}
