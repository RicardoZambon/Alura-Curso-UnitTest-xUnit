using Alura.LeilaoOnline.Core;
using System;
using Xunit;

namespace Alura.LeilaoOnline.Tests
{
    public class LanceCtor
    {
        [Fact]
        public void LancaArgumentExceptionDadoValorNegativo()
        {
            //Arranje
            var valorNegativo = -100;

            //Assert
            var e = Assert.Throws<ArgumentException>(() =>
            {
                //Act
                new Lance(null, valorNegativo);
            });

            var valorEsperado = "Valor do lance deve ser igual ou maior do que zero.";
            Assert.Equal(valorEsperado, e.Message);
        }
    }
}