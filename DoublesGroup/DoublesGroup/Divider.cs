using System;
using System.Collections.Generic;

namespace DoublesGroup
{
    class Divider
    {
        // record games each player has played
        Dictionary<Player, int> m_gamesPlayerHasPlayed;
        List<Player> m_players;
        int m_maxLevelGapBetweenPlayers = 3;

        public List<string> DividePlayers(List<Player> players)
        {
            m_players = players;
            m_gamesPlayerHasPlayed = new Dictionary<Player, int>();
            foreach (Player player in players)
            {
                m_gamesPlayerHasPlayed.Add(player, 0);
            }

            int minGamesPlayerHasPlayed = 0;
            List<Player> playerOrder = new List<Player>();

            while (isFinish() == false)
            {
                List<Player> PlayersToBeGrouped = new List<Player>();
                List<Player> PlayersToBeGrouped2 = new List<Player>();
                foreach (KeyValuePair<Player, int> item in m_gamesPlayerHasPlayed)
                {
                    if (item.Value == minGamesPlayerHasPlayed) PlayersToBeGrouped.Add(item.Key);
                    if (item.Value == minGamesPlayerHasPlayed + 1) PlayersToBeGrouped2.Add(item.Key);
                }

                if (PlayersToBeGrouped.Count > 4)
                {
                    playerOrder.AddRange(GetGroup(new List<Player>(), PlayersToBeGrouped));
                    continue;
                }

                playerOrder.AddRange(GetGroup(PlayersToBeGrouped, PlayersToBeGrouped2));
                minGamesPlayerHasPlayed++;
            }

            List<string> scheduleOrder = new List<string>();

            for (int i = 0; i < playerOrder.Count; i += 4)
            {
                string set = playerOrder[i].Name + " , " + playerOrder[i + 1].Name + "  vs  " + playerOrder[i + 2].Name + " , " + playerOrder[i + 3].Name;
                scheduleOrder.Add(set);
            }

            return scheduleOrder;
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
            int r = random.Next(numberOfPlayer);
            return playersToBeGrouped[r];
        }

        Player Get2ndOr3rdPlayer(Player firstPlayer, List<Player> playersToBeGrouped)
        {
            int numberOfPlayer = playersToBeGrouped.Count;
            if (numberOfPlayer < 1) return playersToBeGrouped[0];
            Random random = new Random();
            int r = random.Next(numberOfPlayer);

            for (int i = 1; i < m_maxLevelGapBetweenPlayers; i++)
            {
                for (int j = r; j < numberOfPlayer + r; j++)
                {
                    bool isChoosePlayerTooPowerless = playersToBeGrouped[j % numberOfPlayer].Level <= firstPlayer.Level + i;
                    bool isChoosePlayerTooPowerful = playersToBeGrouped[j % numberOfPlayer].Level >= firstPlayer.Level - i;
                    if (isChoosePlayerTooPowerless && isChoosePlayerTooPowerful)
                    {
                        return playersToBeGrouped[j % numberOfPlayer];
                    }
                }
            }
            return playersToBeGrouped[r];
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
            int r = random.Next(numberOfPlayer);

            for (int i = 0; i < m_maxLevelGapBetweenPlayers; i++)
            {
                for (int j = r; j < numberOfPlayer + r; j++)
                {
                    bool isPlayerSuitable = playersToBeGrouped[j % numberOfPlayer].Level + MiddleLevel + i == WorstLevel + BestLevel;
                    if (isPlayerSuitable) return playersToBeGrouped[j % numberOfPlayer];
                }
            }
            return playersToBeGrouped[r];
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
