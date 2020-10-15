using Alura.LeilaoOnline.Core;
using System;
using Xunit;

namespace Alura.LeilaoOnline.Tests
{
    public class LeilaoTerminaPregao
    {
        [Theory]
        [InlineData(1200, 1250, new double[] { 800, 1150, 1400, 1250 })]
        public void RetornaValorSuperiorMaisProximoDadoLeilaoNessaModalidade(
            double valorDestino, double valorEsperado, double[] ofertas)
        {
            //Arranje - Cenário
            var leilao = new Leilao("Van Gogh", new OfertaSuperiorMaisProxima(valorDestino));
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.IniciaPregao();
            for (var i = 0; i < ofertas.Length; i++)
            {
                leilao.RecebeLance((i % 2) == 0 ? fulano : maria, ofertas[i]);
            }

            //Act - Método sob teste
            leilao.TerminaPregao();

            //Assert
            Assert.Equal(valorEsperado, leilao.Ganhador.Valor);
        }

        [Theory]
        [InlineData(1200, new double[] { 800, 900, 1000, 1200 })]
        [InlineData(1000, new double[] { 800, 900, 1000, 990 })]
        [InlineData(800, new double[] { 800 })]
        public void RetornaMaioValorDadoLeilaoComPeloMenosUmLance(double valorEsperado, double[] ofertas)
        {
            //Arranje - Cenário
            var leilao = new Leilao("Van Gogh", new MaiorValor());
            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            leilao.IniciaPregao();
            for (var i = 0; i < ofertas.Length; i++)
            {
                leilao.RecebeLance((i % 2) == 0 ? fulano : maria, ofertas[i]);
            }

            //Act - Método sob teste
            leilao.TerminaPregao();

            //Assert
            Assert.Equal(valorEsperado, leilao.Ganhador.Valor);
        }

        [Fact]
        public void LancaInvalidOperationExceptionDadoPregaoNaoIniciado()
        {
            //Arranje - Cenário
            var leilao = new Leilao("Van Gogh", new MaiorValor());

            //Assert
            var excecaoObtida = Assert.Throws<InvalidOperationException>(() =>
            {
                //Act - Método sob teste
                leilao.TerminaPregao();
            });

            var msgEsperada = "Não é possível terminar um pregão sem que ele tenha começado. Para isso, utilize o método IniciaPregao().";
            Assert.Equal(msgEsperada, excecaoObtida.Message);
        }

        [Fact]
        public void RetornaZeroDadoLeilaoSemLances()
        {
            //Arranje - Cenário
            var leilao = new Leilao("Van Gogh", new MaiorValor());

            leilao.IniciaPregao();

            //Act - Método sob teste
            leilao.TerminaPregao();

            //Assert
            var valorEsperado = 0;
            var valorObtido = leilao.Ganhador.Valor;

            Assert.Equal(valorEsperado, valorObtido);
        }
    }
}
