using System;
using Xunit;
using MyTeamWebApi.Interfaces;
using MyTeamWebApi.Model;
using Moq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace MyTeamWebApi.Tests
{
    public class TeamServiceTests
    {
        List<Team> _teamList;
        TeamService _teamService;
        Mock<ITeamFactory> _teamFactoryMock;
        Mock<ILogger<TeamService>> _loggerMock;

        public TeamServiceTests()
        {
            _teamList = new List<Team> {
                new Team { Id = 1, Name = "Sporting", CoachName = "Damasio" },
                new Team { Id = 2, Name = "Benfica", CoachName = "Andre" },
                new Team { Id = 3, Name = "Beira Mar", CoachName = "Mourinho" },
            };

            _teamFactoryMock = new Mock<ITeamFactory>();
            _loggerMock = new Mock<ILogger<TeamService>>();
            _teamService = new TeamService(_teamFactoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void GetTeams_Then_TeamListIsReturned()
        {
            //Arrange
            _teamFactoryMock
                .Setup(p => p.Get(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(_teamList);

            //Act
            var result = _teamService.GetTeams(It.IsAny<string>(), It.IsAny<string>());
            //Console.WriteLine("DEBUG:" + JsonConvert.SerializeObject(result));

            //Assert
            Assert.Equal(result, _teamList);
            _teamFactoryMock.Verify(mock => mock.Get(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        #region GetTeam

        [Fact]
        public void GetTeam_Given_ProvidedTeamIdExists_Then_TeamIsReturned()
        {
            //Arrange
            var expectedTeam = _teamList.ToArray()[0];
            _teamFactoryMock
                .Setup(p => p.Get(It.IsAny<int>()))
                .Returns(expectedTeam);

            //Act
            var result = _teamService.GetTeam(3);
            //Console.WriteLine("DEBUG:" + JsonConvert.SerializeObject(result));

            //Assert
            Assert.Equal(result, expectedTeam);
            _teamFactoryMock.Verify(mock => mock.Get(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public void GetTeam_Given_ProvidedNegativeTeamId_Then_ReturnsNull()
        {
            //Arrange

            //Act
            var result = _teamService.GetTeam(-1);

            //Assert
            Assert.Null(result);
            _teamFactoryMock.Verify(mock => mock.Get(It.IsAny<int>()), Times.Never());
        }

        #endregion

        #region CreateTeam

        [Fact]
        public void CreateTeam_Given_ProvidedAValidTeam_Then_TeamIsSuccessfullyCreated()
        {
            //Arrange
            var newTeam = new Team { Id = 4, Name = "New Team Name" };
            _teamFactoryMock
                .Setup(p => p.Create(It.IsAny<Team>()))
                .Returns(true);

            //Act
            var result = _teamService.CreateTeam(newTeam);

            //Assert
            Assert.True(result);
            _teamFactoryMock.Verify(mock => mock.Create(It.IsAny<Team>()), Times.Once());
        }

        [Fact]
        public void CreateTeam_Given_ProvidedAnEmptyTeamName_Then_TeamIsNotCreated()
        {
            //Arrange
            var invalidTeam = new Team { Name = " ", Id = 1 };
            //Act
            var result = _teamService.CreateTeam(invalidTeam);

            //Assert
            Assert.False(result);
            _teamFactoryMock.Verify(mock => mock.Create(invalidTeam), Times.Never());
        }

        [Fact]
        public void CreateTeam_Given_ProvidedANullTeam_Then_ReturnsFalse()
        {
            //Arrange
            Team invalidTeam = null;
            //Act
            var result = _teamService.CreateTeam(invalidTeam);

            //Assert
            Assert.False(result);
            _teamFactoryMock.Verify(mock => mock.Create(invalidTeam), Times.Never());
        }

        [Fact]
        public void CreateTeam_Given_ProvidedAnInvalidIdTeamNumber_Then_ReturnsFalse()
        {
            //Arrange
            var invalidTeam = new Team { Name = "team's name", Id = -1 };
            //Act
            var result = _teamService.CreateTeam(invalidTeam);

            //Assert
            Assert.False(result);
            _teamFactoryMock.Verify(mock => mock.Create(invalidTeam), Times.Never());
        }

        #endregion

        #region UpdateTeam

        [Fact]
        public void UpdateTeam_Given_ProvidedValidTeamData_Then_TeamDataIsSuccessfullyUpdated()
        {
            //Arrange
            var newTeam = new Team { Name = "New Team Name" };
            _teamFactoryMock
                .Setup(p => p.Update(It.IsAny<int>(), It.IsAny<Team>()))
                .Returns(true);

            var expectedTeam = _teamList.ToArray()[0];
            _teamFactoryMock
                .Setup(p => p.Get(It.IsAny<int>()))
                .Returns(expectedTeam);

            //Act
            var result = _teamService.UpdateTeam(1, newTeam);

            //Assert
            Assert.True(result);
            _teamFactoryMock.Verify(mock => mock.Update(It.IsAny<int>(), It.IsAny<Team>()), Times.Once());
            _teamFactoryMock.Verify(mock => mock.Get(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public void UpdateTeam_Given_ProvidedNegativeTeamId_Then_TeamDataIsNotUpdated()
        {
            //Arrange
            var newTeamData = new Team { Name = "A new team name" };
            //Act
            var result = _teamService.UpdateTeam(-1, newTeamData);
            //Assert
            Assert.False(result);
            _teamFactoryMock.Verify(mock => mock.Update(It.IsAny<int>(), It.IsAny<Team>()), Times.Never());
            _teamFactoryMock.Verify(mock => mock.Get(It.IsAny<int>()), Times.Never());
        }

        [Fact]
        public void UpdateTeam_Given_ProvidedInvalidTeamId_Then_TeamDataIsNotUpdated()
        {
            //Arrange

            //Act
            var result = _teamService.UpdateTeam(-1, It.IsAny<Team>());
            //Assert
            Assert.False(result);
            _teamFactoryMock.Verify(mock => mock.Update(It.IsAny<int>(), It.IsAny<Team>()), Times.Never());
            _teamFactoryMock.Verify(mock => mock.Get(It.IsAny<int>()), Times.Never());
        }

        [Fact]
        public void UpdateTeam_Given_ProvidedNullTeam_Then_TeamDataIsNotUpdated()
        {
            //Arrange

            //Act
            var result = _teamService.UpdateTeam(1, null);
            //Assert
            Assert.False(result);
            _teamFactoryMock.Verify(mock => mock.Update(It.IsAny<int>(), It.IsAny<Team>()), Times.Never());
            _teamFactoryMock.Verify(mock => mock.Get(It.IsAny<int>()), Times.Never());
        }

        [Fact]
        public void UpdateTeam_Given_ProvidedEmptyTeamName_Then_TeamDataIsNotUpdated()
        {
            //Arrange
            var newTeamData = new Team { Name = " " };

            //Act
            var result = _teamService.UpdateTeam(1, newTeamData);
            //Assert
            Assert.False(result);
            _teamFactoryMock.Verify(mock => mock.Update(It.IsAny<int>(), It.IsAny<Team>()), Times.Never());
            _teamFactoryMock.Verify(mock => mock.Get(It.IsAny<int>()), Times.Never());
        }

        #endregion

        #region DeleteTeam

        [Fact]
        public void DeleteTeam_Given_ProvidedValidTeamId_Then_TeamStatusIsSuccessfullyUpdated()
        {
            //Arrange
            _teamFactoryMock
                .Setup(p => p.Update(It.IsAny<int>(), It.IsAny<Team>()))
                .Returns(true);

            var expectedTeam = _teamList.ToArray()[0];
            _teamFactoryMock
                .Setup(p => p.Get(It.IsAny<int>()))
                .Returns(expectedTeam);

            //Act
            var result = _teamService.DeleteTeam(1);

            //Assert
            Assert.True(result);
            _teamFactoryMock.Verify(mock => mock.Update(It.IsAny<int>(), It.IsAny<Team>()), Times.Once());
            _teamFactoryMock.Verify(mock => mock.Get(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public void DeleteTeam_Given_ProvidedInvalidTeamId_Then_TeamDataIsNotUpdated()
        {
            //Arrange

            //Act
            var result = _teamService.DeleteTeam(-1);
            //Assert
            Assert.False(result);
            _teamFactoryMock.Verify(mock => mock.Update(It.IsAny<int>(), It.IsAny<Team>()), Times.Never());
            _teamFactoryMock.Verify(mock => mock.Get(It.IsAny<int>()), Times.Never());
        }

        #endregion

        #region AddMatch

        [Fact]
        public void AddMatch_Given_ProvidedValidTeamIdAndMatchResult_Then_TeamMatchListIsSuccessfullyUpdated()
        {
            //Arrange
            _teamFactoryMock
                .Setup(p => p.AddMatch(It.IsAny<int>(), It.IsAny<MatchResultType>()))
                .Returns(true);

            //Act
            var result = _teamService.AddMatch(1, MatchResultType.Win);

            //Assert
            Assert.True(result);
            _teamFactoryMock.Verify(mock => mock.AddMatch(It.IsAny<int>(), It.IsAny<MatchResultType>()), Times.Once());
        }

        [Fact]
        public void AddMatch_Given_ProvidedInvalidTeamId_Then_TeamMatchListIsNotUpdated()
        {
            //Arrange

            //Act
            var result = _teamService.AddMatch(-1, MatchResultType.Win);
            //Assert
            Assert.False(result);
            _teamFactoryMock.Verify(mock => mock.AddMatch(It.IsAny<int>(), It.IsAny<MatchResultType>()), Times.Never());
        }

        [Fact]
        public void AddMatch_Given_ProvidedInvalidMatchResultType_Then_TeamMatchListIsNotUpdated()
        {
            //Arrange

            //Act
            var result = _teamService.AddMatch(1, MatchResultType.All);
            //Assert
            Assert.False(result);
            _teamFactoryMock.Verify(mock => mock.AddMatch(It.IsAny<int>(), It.IsAny<MatchResultType>()), Times.Never());
        }

        #endregion

        #region GetMatchesTotal

        [Fact]
        public void GetMatchesTotal_Given_ProvidedValidTeamIdAndMatchResult_Then_TeamMatchTotalsAreReturned()
        {
            //Arrange
            _teamFactoryMock
                .Setup(p => p.GetMatchesTotal(It.IsAny<int>(), It.IsAny<MatchResultType>()))
                .Returns(It.IsAny<int>());

            //Act
            var result = _teamService.GetMatchesTotal(1, MatchResultType.Win);

            //Assert
            Assert.Equal(result, It.IsAny<int>());
            _teamFactoryMock.Verify(mock => mock.GetMatchesTotal(It.IsAny<int>(), It.IsAny<MatchResultType>()), Times.Once());
        }

        [Fact]
        public void GetMatchesTotal_Given_ProvidedInvalidTeamId_Then_TeamMatchTotalsReturnZero()
        {
            //Arrange

            //Act
            var result = _teamService.GetMatchesTotal(-1, It.IsAny<MatchResultType>());
            //Assert
            Assert.Equal(result, It.IsAny<int>());
            _teamFactoryMock.Verify(mock => mock.GetMatchesTotal(It.IsAny<int>(), It.IsAny<MatchResultType>()), Times.Never());
        }

        #endregion

    }
}
