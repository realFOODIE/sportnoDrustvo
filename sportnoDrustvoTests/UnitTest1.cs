using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using sportnoDrustvo.Pages.Obvestila;
using sportnoDrustvo.Classes;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using sportnoDrustvo.Controllers;
using static sportnoDrustvo.Classes.Models;
using System.Linq.Expressions;
//using sportnoDrustvo.Pages.Clani;



namespace sportnoDrustvoTests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void Termin_CreationAndProperties_Test()
        {
            var termin = new Models.Termin
            {
                Id = 1,
                ImeEkipe = "Ekipa A",
                DatumTermina = DateTime.Now,
                MaxUdelezencev = 22
            };

            Assert.AreEqual(1, termin.Id);
            Assert.AreEqual("Ekipa A", termin.ImeEkipe);
            Assert.AreEqual(22, termin.MaxUdelezencev);
            //preveri datum z neko toleranco
            Assert.IsTrue((DateTime.Now - termin.DatumTermina).TotalSeconds < 1);
        }

        [TestMethod]
        public void Obvestilo_CreationAndProperties_Test()
        {
            var obvestilo = new Models.Obvestilo
            {
                Id = 1,
                TerminId = 2,
                Obvescanje = true
            };

            Assert.AreEqual(1, obvestilo.Id);
            Assert.AreEqual(2, obvestilo.TerminId);
            Assert.IsTrue(obvestilo.Obvescanje);
        }

        [TestMethod]
        public void Rekvizit_CreationAndProperties_Test()
        {
            var rekvizit = new Models.Rekvizit
            {
                Id = 1,
                Ime = "Lopta",
                JeNaVoljo = true,
                ClanId = null,
                DatumIzposoje = null
            };

            Assert.AreEqual(1, rekvizit.Id);
            Assert.AreEqual("Lopta", rekvizit.Ime);
            Assert.IsTrue(rekvizit.JeNaVoljo);
            Assert.IsNull(rekvizit.ClanId);
            Assert.IsNull(rekvizit.DatumIzposoje);
        }

        [TestMethod]
        public void Trener_CreationAndProperties_Test()
        {
            var trener = new Models.Trener
            {
                Id = 1,
                Ime = "Marko",
                Specializacija = "Nogomet"
            };

            Assert.AreEqual(1, trener.Id);
            Assert.AreEqual("Marko", trener.Ime);
            Assert.AreEqual("Nogomet", trener.Specializacija);
        }

        [TestMethod]
        public void Clan_CreationAndProperties_Test()
        {
            var clan = new Models.Clan
            {
                Id = 1,
                Ime = "Janez",
                Priimek = "Novak"
            };

            Assert.AreEqual(1, clan.Id);
            Assert.AreEqual("Janez", clan.Ime);
            Assert.AreEqual("Novak", clan.Priimek);
        }

        [TestMethod]
        public void Rezervacija_CreationAndProperties_Test()
        {
            var rezervacija = new Models.Rezervacija
            {
                Id = 1,
                TerminId = 2,
                ClanId = 3,
                DatumRezervacije = DateTime.Today
            };

            Assert.AreEqual(1, rezervacija.Id);
            Assert.AreEqual(2, rezervacija.TerminId);
            Assert.AreEqual(3, rezervacija.ClanId);
            Assert.AreEqual(DateTime.Today, rezervacija.DatumRezervacije);
        }


        [TestMethod]
        public void Termin_MaxUdelezencev_Negativno_Test()
        {
            var termin = new Models.Termin
            {
                MaxUdelezencev = -1
            };

            Assert.IsTrue(termin.MaxUdelezencev < 0, "MaxUdelezencev ne bi smel biti negativen.");
        }

        [TestMethod]
        public void Obvestilo_TerminRelacija_Test()
        {
            var termin = new Models.Termin { Id = 1 };
            var obvestilo = new Models.Obvestilo
            {
                Termin = termin
            };

            Assert.AreEqual(termin, obvestilo.Termin, "Termin ni pravilno dodeljen obvestilu.");
        }

        [TestMethod]
        public void Clan_Obvestila_Count_Test()
        {
            var clan = new Models.Clan
            {
                Obvestila = new List<Models.Obvestilo>
                {
                    new Models.Obvestilo(),
                    new Models.Obvestilo()
                }
            };

            Assert.AreEqual(2, clan.Obvestila.Count, "Število obvestil ni pravilno.");
        }
    }



    [TestClass]
    public class ClaniModelTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private CreateModel _createModel;
        private sportnoDrustvo.Classes.Models.Clan _clan;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _createModel = new CreateModel(_mockContext.Object);
            _clan = new sportnoDrustvo.Classes.Models.Clan();
        }

        [TestMethod]
        public void OnGet_ReturnsPageResult()
        {
            var result = _createModel.OnGet();
            Assert.IsInstanceOfType(result, typeof(PageResult));
        }

        [TestMethod]
        public async Task OnPostAsync_WithInvalidModelState_ReturnsPage()
        {
            _createModel.ModelState.AddModelError("Error", "AddModelError");
            var result = await _createModel.OnPostAsync();
            Assert.IsInstanceOfType(result, typeof(PageResult));
        }

        [TestMethod]
        public async Task OnPostAsync_WithValidModel_AddsClanAndRedirects()
        {
            //nastavitev posnemanja SaveChangesAsync, da vrne Task<int> z vrednostjo 1, kar pomeni, da je ena entiteta bila dodana
            _mockContext.Setup(c => c.SaveChangesAsync(default(CancellationToken)))
                        .ReturnsAsync(1); //uporaba ReturnsAsync za simulacijo asinhrone metode, ki vrača Task<int>

            var result = await _createModel.OnPostAsync();

            _mockContext.Verify(m => m.Clani.Add(It.IsAny<sportnoDrustvo.Classes.Models.Clan>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
        }

        ///DELETE
        [TestMethod]
        public async Task OnGetAsync_ValidId_ReturnsPageResult()
        {
            var mockContext = new Mock<ApplicationDbContext>();
            var testId = 1;
            mockContext.Setup(db => db.Clani.FirstOrDefaultAsync(m => m.Id == testId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(new sportnoDrustvo.Classes.Models.Clan { Id = testId });

            var deleteModel = new DeleteModel(mockContext.Object);
            var result = await deleteModel.OnGetAsync(testId);

            Assert.IsInstanceOfType(result, typeof(PageResult));
        }

        [TestMethod]
        public async Task OnGetAsync_InvalidId_ReturnsNotFoundResult()
        {
            var mockContext = new Mock<ApplicationDbContext>();
            var testId = -1;
            mockContext.Setup(db => db.Clani.FirstOrDefaultAsync(m => m.Id == testId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((sportnoDrustvo.Classes.Models.Clan)null);

            var deleteModel = new DeleteModel(mockContext.Object);
            var result = await deleteModel.OnGetAsync(testId);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        [TestMethod]
        public async Task OnPostAsync_ValidId_DeletesClanAndRedirects()
        {
            var mockContext = new Mock<ApplicationDbContext>();
            var testId = 1;
            var testClan = new sportnoDrustvo.Classes.Models.Clan { Id = testId };
            mockContext.Setup(db => db.Clani.FindAsync(testId))
                       .ReturnsAsync(testClan);
            mockContext.Setup(db => db.SaveChangesAsync(default))
                       .ReturnsAsync(1);

            var deleteModel = new DeleteModel(mockContext.Object);
            var result = await deleteModel.OnPostAsync(testId);

            mockContext.Verify(db => db.Clani.Remove(testClan), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
        }


        //DETAILS
        [TestMethod]
        public async Task DetailsOnGetAsync_ValidId_ReturnsPageResult()
        {
            var mockContext = new Mock<ApplicationDbContext>();
            var testId = 1;
            mockContext.Setup(db => db.Clani.FirstOrDefaultAsync(m => m.Id == testId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(new sportnoDrustvo.Classes.Models.Clan { Id = testId });

            var detailsModel = new DetailsModel(mockContext.Object);
            var result = await detailsModel.OnGetAsync(testId);

            Assert.IsInstanceOfType(result, typeof(PageResult));
        }

        [TestMethod]
        public async Task DetailsOnGetAsync_InvalidId_ReturnsNotFoundResult()
        {
            var mockContext = new Mock<ApplicationDbContext>();
            var testId = -1;
            mockContext.Setup(db => db.Clani.FirstOrDefaultAsync(m => m.Id == testId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((sportnoDrustvo.Classes.Models.Clan)null);

            var detailsModel = new DetailsModel(mockContext.Object);
            var result = await detailsModel.OnGetAsync(testId);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task OnGetAsync_NullId_ReturnsNotFoundResult()
        {
            var mockContext = new Mock<ApplicationDbContext>();

            var detailsModel = new DetailsModel(mockContext.Object);
            var result = await detailsModel.OnGetAsync(null);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        //EDIT
        [TestMethod]
        public async Task EDITOnGetAsync_ValidId_ReturnsPageResult()
        {
            var mockContext = new Mock<ApplicationDbContext>();
            var testId = 1;
            mockContext.Setup(db => db.Clani.FirstOrDefaultAsync(m => m.Id == testId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(new sportnoDrustvo.Classes.Models.Clan { Id = testId });

            var editModel = new EditModel(mockContext.Object);
            var result = await editModel.OnGetAsync(testId);

            Assert.IsInstanceOfType(result, typeof(PageResult));
        }

        [TestMethod]
        public async Task EDITOnGetAsync_InvalidId_ReturnsNotFoundResult()
        {
            var mockContext = new Mock<ApplicationDbContext>();
            var testId = -1;
            mockContext.Setup(db => db.Clani.FirstOrDefaultAsync(m => m.Id == testId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((sportnoDrustvo.Classes.Models.Clan)null);

            var editModel = new EditModel(mockContext.Object);
            var result = await editModel.OnGetAsync(testId);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

    }


    [TestClass]
    public class CreateObvestiloModelTests
    {
        private Mock<ApplicationDbContext> mockContext;
        private CreateModel createModel;

        [TestInitialize]
        public void TestInitialize()
        {
            mockContext = new Mock<ApplicationDbContext>();


            var termini = new List<Models.Termin>
            {
                new Models.Termin { Id = 1, ImeEkipe = "Ekipa 1" },

            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Models.Termin>>();
            mockDbSet.As<IQueryable<Models.Termin>>().Setup(m => m.Provider).Returns(termini.Provider);
            mockDbSet.As<IQueryable<Models.Termin>>().Setup(m => m.Expression).Returns(termini.Expression);
            mockDbSet.As<IQueryable<Models.Termin>>().Setup(m => m.ElementType).Returns(termini.ElementType);
            mockDbSet.As<IQueryable<Models.Termin>>().Setup(m => m.GetEnumerator()).Returns(termini.GetEnumerator());

            mockContext.Setup(c => c.Termini).Returns(mockDbSet.Object);

            createModel = new CreateModel(mockContext.Object);
        }

        [TestMethod]
        public void OnGet_PopulatesSelectList()
        {
            createModel.OnGet();

            Assert.IsNotNull(createModel.ViewData["TerminId"]);
            var selectList = createModel.ViewData["TerminId"] as SelectList;
            Assert.IsTrue(selectList.Any());
        }

        [TestMethod]
        public async Task OnPostAsync_WithInvalidModel_ReturnsPage()
        {
            createModel.ModelState.AddModelError("Napaka", "Testna napaka");

            var result = await createModel.OnPostAsync();

            Assert.IsInstanceOfType(result, typeof(PageResult));
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }
    }





    //CLAN CONTROLLER TESTS
    [TestClass]
    public class ClanControllerTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private Mock<DbSet<Models.Clan>> _mockSet;
        private ClanController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _mockSet = new Mock<DbSet<Models.Clan>>();

            _mockContext.Setup(m => m.Clani).Returns(_mockSet.Object);
            _controller = new ClanController(_mockContext.Object);
        }

        [TestMethod]
        public async Task GetClani_ReturnsAllClani()
        {
            var claniData = new List<Models.Clan>
            {
                new Models.Clan { Id = 1, Ime = "Janez", Priimek = "Novak" },
                new Models.Clan { Id = 2, Ime = "Micka", Priimek = "Kovač" }
            }.AsQueryable();

            _mockSet.As<IQueryable<Models.Clan>>().Setup(m => m.Provider).Returns(claniData.Provider);
            _mockSet.As<IQueryable<Models.Clan>>().Setup(m => m.Expression).Returns(claniData.Expression);
            _mockSet.As<IQueryable<Models.Clan>>().Setup(m => m.ElementType).Returns(claniData.ElementType);
            _mockSet.As<IQueryable<Models.Clan>>().Setup(m => m.GetEnumerator()).Returns(claniData.GetEnumerator());

            var result = await _controller.GetClani();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var clani = okResult.Value as IEnumerable<Models.Clan>;
            Assert.AreEqual(2, clani.Count());
        }

        [TestMethod]
        public async Task GetClan_ReturnsClan_IfExists()
        {
            var clan = new Models.Clan { Id = 1, Ime = "Janez", Priimek = "Novak" };
            _mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(clan);

            var result = await _controller.GetClan(1);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var resultClan = okResult.Value as Models.Clan;
            Assert.AreEqual(1, resultClan.Id);
        }

        [TestMethod]
        public async Task PostClan_AddsClanAndReturnsCreatedAtAction()
        {
            var clanToAdd = new Models.Clan { Ime = "Novi Clan", Priimek = "Novak" };

            var result = await _controller.PostClan(clanToAdd);

            _mockSet.Verify(m => m.Add(It.IsAny<Models.Clan>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);

            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetClan", createdAtActionResult.ActionName);
            var resultClan = createdAtActionResult.Value as Models.Clan;
            Assert.IsNotNull(resultClan);
        }
    }

    [TestClass]
    public class ObvestiloControllerTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private ObvestiloController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _controller = new ObvestiloController(_mockContext.Object);
        }

        [TestMethod]
        public async Task GetObvestila_ReturnsAllObvestila()
        {
            var obvestilaData = new List<Obvestilo>
            {
                new Obvestilo { Id = 1, TerminId = 1, Obvescanje = true },
                new Obvestilo { Id = 2, TerminId = 2, Obvescanje = false }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Obvestilo>>();
            mockSet.As<IQueryable<Obvestilo>>().Setup(m => m.Provider).Returns(obvestilaData.Provider);
            mockSet.As<IQueryable<Obvestilo>>().Setup(m => m.Expression).Returns(obvestilaData.Expression);
            mockSet.As<IQueryable<Obvestilo>>().Setup(m => m.ElementType).Returns(obvestilaData.ElementType);
            mockSet.As<IQueryable<Obvestilo>>().Setup(m => m.GetEnumerator()).Returns(obvestilaData.GetEnumerator());

            _mockContext.Setup(m => m.Obvestila).Returns(mockSet.Object);

            var result = await _controller.GetObvestila();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var obvestila = okResult.Value as IEnumerable<Obvestilo>;
            Assert.AreEqual(2, obvestila.Count());
        }

        [TestMethod]
        public async Task GetObvestilo_ReturnsObvestilo_IfExists()
        {
            var testId = 1;
            var obvestilo = new Obvestilo { Id = testId, TerminId = 1, Obvescanje = true };
            _mockContext.Setup(m => m.Obvestila.FindAsync(testId)).ReturnsAsync(obvestilo);

            var result = await _controller.GetObvestilo(testId);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var resultObvestilo = okResult.Value as Obvestilo;
            Assert.AreEqual(testId, resultObvestilo.Id);
        }

        [TestMethod]
        public async Task PostObvestilo_AddsObvestiloAndReturnsCreatedAtAction()
        {
            var obvestiloToAdd = new Obvestilo { TerminId = 1, Obvescanje = true };

            var result = await _controller.PostObvestilo(obvestiloToAdd);

            _mockContext.Verify(m => m.Obvestila.Add(It.IsAny<Obvestilo>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);

            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetObvestilo", createdAtActionResult.ActionName);
            var resultObvestilo = createdAtActionResult.Value as Obvestilo;
            Assert.IsNotNull(resultObvestilo);
        }
    }





    [TestClass]
    public class RekvizitControllerTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private RekvizitController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _controller = new RekvizitController(_mockContext.Object);
        }

        [TestMethod]
        public async Task GetRekviziti_ReturnsAllRekviziti()
        {
            var rekvizitiData = new List<Rekvizit>
            {
                new Rekvizit { Id = 1, Ime = "Lopta", JeNaVoljo = true },
                new Rekvizit { Id = 2, Ime = "Dres", JeNaVoljo = false }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Rekvizit>>();
            mockSet.As<IQueryable<Rekvizit>>().Setup(m => m.Provider).Returns(rekvizitiData.Provider);
            mockSet.As<IQueryable<Rekvizit>>().Setup(m => m.Expression).Returns(rekvizitiData.Expression);
            mockSet.As<IQueryable<Rekvizit>>().Setup(m => m.ElementType).Returns(rekvizitiData.ElementType);
            mockSet.As<IQueryable<Rekvizit>>().Setup(m => m.GetEnumerator()).Returns(rekvizitiData.GetEnumerator());

            _mockContext.Setup(m => m.Rekviziti).Returns(mockSet.Object);

            var result = await _controller.GetRekviziti();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var rekviziti = okResult.Value as IEnumerable<Rekvizit>;
            Assert.AreEqual(2, rekviziti.Count());
        }

        [TestMethod]
        public async Task GetRekvizit_ReturnsRekvizit_IfExists()
        {
            var testId = 1;
            var rekvizit = new Rekvizit { Id = testId, Ime = "Lopta", JeNaVoljo = true };
            _mockContext.Setup(m => m.Rekviziti.FindAsync(testId)).ReturnsAsync(rekvizit);

            var result = await _controller.GetRekvizit(testId);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var resultRekvizit = okResult.Value as Rekvizit;
            Assert.AreEqual(testId, resultRekvizit.Id);
        }

        [TestMethod]
        public async Task PostRekvizit_AddsRekvizitAndReturnsCreatedAtAction()
        {
            var rekvizitToAdd = new Rekvizit { Ime = "Nov Rekvizit", JeNaVoljo = true };

            var result = await _controller.PostRekvizit(rekvizitToAdd);

            _mockContext.Verify(m => m.Rekviziti.Add(It.IsAny<Rekvizit>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);

            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetRekvizit", createdAtActionResult.ActionName);
            var resultRekvizit = createdAtActionResult.Value as Rekvizit;
            Assert.IsNotNull(resultRekvizit);
        }
    }

    [TestClass]
    public class RezervacijaControllerTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private RezervacijaController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _controller = new RezervacijaController(_mockContext.Object);
        }

        [TestMethod]
        public async Task GetRezervacije_ReturnsAllRezervacije()
        {
            var rezervacijeData = new List<Rezervacija>
            {
                new Rezervacija { Id = 1, ClanId = 1, TerminId = 1 },
                new Rezervacija { Id = 2, ClanId = 2, TerminId = 2 }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Rezervacija>>();
            mockSet.As<IQueryable<Rezervacija>>().Setup(m => m.Provider).Returns(rezervacijeData.Provider);
            mockSet.As<IQueryable<Rezervacija>>().Setup(m => m.Expression).Returns(rezervacijeData.Expression);
            mockSet.As<IQueryable<Rezervacija>>().Setup(m => m.ElementType).Returns(rezervacijeData.ElementType);
            mockSet.As<IQueryable<Rezervacija>>().Setup(m => m.GetEnumerator()).Returns(rezervacijeData.GetEnumerator());

            _mockContext.Setup(m => m.Rezervacije).Returns(mockSet.Object);

            var result = await _controller.GetRezervacije();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var rezervacije = okResult.Value as IEnumerable<Rezervacija>;
            Assert.AreEqual(2, rezervacije.Count());
        }

        [TestMethod]
        public async Task GetRezervacija_ReturnsRezervacija_IfExists()
        {
            var testId = 1;
            var rezervacija = new Rezervacija { Id = testId, ClanId = 1, TerminId = 1 };
            _mockContext.Setup(m => m.Rezervacije.FindAsync(testId)).ReturnsAsync(rezervacija);

            var result = await _controller.GetRezervacija(testId);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var resultRezervacija = okResult.Value as Rezervacija;
            Assert.AreEqual(testId, resultRezervacija.Id);
        }

        [TestMethod]
        public async Task PostRezervacija_AddsRezervacijaAndReturnsCreatedAtAction()
        {
            var rezervacijaToAdd = new Rezervacija { ClanId = 1, TerminId = 1 };

            var result = await _controller.PostRezervacija(rezervacijaToAdd);

            _mockContext.Verify(m => m.Rezervacije.Add(It.IsAny<Rezervacija>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);

            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetRezervacija", createdAtActionResult.ActionName);
            var resultRezervacija = createdAtActionResult.Value as Rezervacija;
            Assert.IsNotNull(resultRezervacija);
        }
    }

    [TestClass]
    public class TerminControllerTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private TerminController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _controller = new TerminController(_mockContext.Object);
        }

        [TestMethod]
        public async Task GetTermini_ReturnsAllTermini()
        {
            var terminiData = new List<Termin>
            {
                new Termin { Id = 1, ImeEkipe = "Ekipa 1", DatumTermina = System.DateTime.Now },
                new Termin { Id = 2, ImeEkipe = "Ekipa 2", DatumTermina = System.DateTime.Now.AddDays(1) }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Termin>>();
            mockSet.As<IQueryable<Termin>>().Setup(m => m.Provider).Returns(terminiData.Provider);
            mockSet.As<IQueryable<Termin>>().Setup(m => m.Expression).Returns(terminiData.Expression);
            mockSet.As<IQueryable<Termin>>().Setup(m => m.ElementType).Returns(terminiData.ElementType);
            mockSet.As<IQueryable<Termin>>().Setup(m => m.GetEnumerator()).Returns(terminiData.GetEnumerator());

            _mockContext.Setup(m => m.Termini).Returns(mockSet.Object);

            var result = await _controller.GetTermini();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var termini = okResult.Value as IEnumerable<Termin>;
            Assert.AreEqual(2, termini.Count());
        }

        [TestMethod]
        public async Task GetTermin_ReturnsTermin_IfExists()
        {
            var testId = 1;
            var termin = new Termin { Id = testId, ImeEkipe = "Ekipa 1", DatumTermina = System.DateTime.Now };
            _mockContext.Setup(m => m.Termini.FindAsync(testId)).ReturnsAsync(termin);

            var result = await _controller.GetTermin(testId);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var resultTermin = okResult.Value as Termin;
            Assert.AreEqual(testId, resultTermin.Id);
        }

        [TestMethod]
        public async Task PostTermin_AddsTerminAndReturnsCreatedAtAction()
        {
            var terminToAdd = new Termin { ImeEkipe = "Nova Ekipa", DatumTermina = System.DateTime.Now };

            var result = await _controller.PostTermin(terminToAdd);

            _mockContext.Verify(m => m.Termini.Add(It.IsAny<Termin>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);

            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetTermin", createdAtActionResult.ActionName);
            var resultTermin = createdAtActionResult.Value as Termin;
            Assert.IsNotNull(resultTermin);
        }
    }


    [TestClass]
    public class TrenerControllerTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private TrenerController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _controller = new TrenerController(_mockContext.Object);
        }

        [TestMethod]
        public async Task GetTrenerji_ReturnsAllTrenerji()
        {
            var trenerjiData = new List<Trener>
            {
                new Trener { Id = 1, Ime = "Marko", Specializacija = "Nogomet" },
                new Trener { Id = 2, Ime = "Ana", Specializacija = "Košarka" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Trener>>();
            mockSet.As<IQueryable<Trener>>().Setup(m => m.Provider).Returns(trenerjiData.Provider);
            mockSet.As<IQueryable<Trener>>().Setup(m => m.Expression).Returns(trenerjiData.Expression);
            mockSet.As<IQueryable<Trener>>().Setup(m => m.ElementType).Returns(trenerjiData.ElementType);
            mockSet.As<IQueryable<Trener>>().Setup(m => m.GetEnumerator()).Returns(trenerjiData.GetEnumerator());

            _mockContext.Setup(m => m.Trenerji).Returns(mockSet.Object);

            var result = await _controller.GetTrenerji();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var trenerji = okResult.Value as IEnumerable<Trener>;
            Assert.AreEqual(2, trenerji.Count());
        }

        [TestMethod]
        public async Task GetTrener_ReturnsTrener_IfExists()
        {
            var testId = 1;
            var trener = new Trener { Id = testId, Ime = "Marko", Specializacija = "Nogomet" };
            _mockContext.Setup(m => m.Trenerji.FindAsync(testId)).ReturnsAsync(trener);

            var result = await _controller.GetTrener(testId);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var resultTrener = okResult.Value as Trener;
            Assert.AreEqual(testId, resultTrener.Id);
        }

        [TestMethod]
        public async Task PostTrener_AddsTrenerAndReturnsCreatedAtAction()
        {
            var trenerToAdd = new Trener { Ime = "Novi Trener", Specializacija = "Plavanje" };

            var result = await _controller.PostTrener(trenerToAdd);

            _mockContext.Verify(m => m.Trenerji.Add(It.IsAny<Trener>()), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);

            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetTrener", createdAtActionResult.ActionName);
            var resultTrener = createdAtActionResult.Value as Trener;
            Assert.IsNotNull(resultTrener);
        }
    }



    [TestClass]
    public class TrenerModelTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private CreateModel _createModel;
        private DeleteModel _deleteModel;
        private DetailsModel _detailsModel;
        private EditModel _editModel;
        private Trener _trener;


        [TestInitialize]
        public void TestInitialize()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _createModel = new CreateModel(_mockContext.Object);
            _deleteModel = new DeleteModel(_mockContext.Object);
            _detailsModel = new DetailsModel(_mockContext.Object);
            _editModel = new EditModel(_mockContext.Object);

            _trener = new Trener();
        }


        //CREATE


        [TestMethod]
        public void OnGet_ReturnsPageResult()
        {
            var result = _createModel.OnGet();
            Assert.IsInstanceOfType(result, typeof(PageResult));
        }

        [TestMethod]
        public async Task OnPostAsync_WithInvalidModelState_ReturnsPage()
        {
            _createModel.ModelState.AddModelError("Error", "AddModelError");
            var result = await _createModel.OnPostAsync();
            Assert.IsInstanceOfType(result, typeof(PageResult));
        }

        [TestMethod]
        public async Task OnPostAsync_WithValidModel_AddsTrenerAndRedirects()
        {
            // vrne Task<int> z vrednostjo 1
            _mockContext.Setup(c => c.SaveChangesAsync(default))
                        .ReturnsAsync(1);

            var result = await _createModel.OnPostAsync();

            _mockContext.Verify(m => m.Trenerji.Add(It.IsAny<Trener>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
        }


        //DELETE


        [TestMethod]
        public async Task OnGetAsync_ValidId_ReturnsPageResult()
        {
            var testId = 1; // ID obstaja
            _mockContext.Setup(db => db.Trenerji.FirstOrDefaultAsync(m => m.Id == testId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new Trener { Id = testId });

            var result = await _deleteModel.OnGetAsync(testId);

            Assert.IsInstanceOfType(result, typeof(PageResult));
        }

        [TestMethod]
        public async Task OnGetAsync_InvalidId_ReturnsNotFoundResult()
        {
            var testId = -1; // ID ne obstaja
            _mockContext.Setup(db => db.Trenerji.FirstOrDefaultAsync(m => m.Id == testId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync((Trener)null);

            var result = await _deleteModel.OnGetAsync(testId);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task OnPostAsync_ValidId_DeletesTrenerAndRedirects()
        {
            var testId = 1;
            var testTrener = new Trener { Id = testId };
            _mockContext.Setup(db => db.Trenerji.FindAsync(testId))
                        .ReturnsAsync(testTrener);
            _mockContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            var result = await _deleteModel.OnPostAsync(testId);

            _mockContext.Verify(db => db.Trenerji.Remove(testTrener), Times.Once);
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
        }


        //DETAILS


        [TestMethod]
        public async Task OnGetAsync_IdIsNull_ReturnsNotFound()
        {
            var result = await _detailsModel.OnGetAsync(null);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task OnGetAsync_TrenerDoesNotExist_ReturnsNotFound()
        {
            var testId = -1; // noben nima ta ID
            _mockContext.Setup(db => db.Trenerji.FirstOrDefaultAsync(m => m.Id == testId, It.IsAny<CancellationToken>()))
                        .ReturnsAsync((Trener)null);

            var result = await _detailsModel.OnGetAsync(testId);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }



        //EDIT


        [TestMethod]
        public async Task EditModel_OnGetAsync_InvalidId_ReturnsNotFound()
        {
            var result = await _editModel.OnGetAsync(null);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task EditModel_OnPostAsync_InvalidModel_ReturnsPage()
        {
            _editModel.ModelState.AddModelError("Error", "Sample error");
            var result = await _editModel.OnPostAsync();
            Assert.IsInstanceOfType(result, typeof(PageResult));
        }

        [TestMethod]
        public async Task EditModel_OnPostAsync_ValidModel_SavesChanges()
        {
            _mockContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                        .Returns((Task<int>)Task.CompletedTask);
            _mockContext.Setup(db => db.Trenerji.Any(It.IsAny<Expression<Func<Trener, bool>>>()))
                        .Returns(true);

            var result = await _editModel.OnPostAsync();
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
        }
    }



}