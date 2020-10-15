using System;
using System.Collections.Generic;

namespace Alura.LeilaoOnline.Core
{
    public enum EstadoLeilao
    {
        LeilaoAntesDoPregao,
        LeilaoEmAndamento,
        LeilaoFinalizado
    }

    public class Leilao
    {
        private Interessada _ultimoCliente;
        private IList<Lance> _lances = new List<Lance>();
        private IModalidadeAvaliacao _avaliador;

        public IEnumerable<Lance> Lances => _lances;
        public string Peca { get; }
        public Lance Ganhador { get; private set; }
        public EstadoLeilao Estado { get; private set; } = EstadoLeilao.LeilaoAntesDoPregao;


        public Leilao(string peca, IModalidadeAvaliacao modalidadeAvaliacao)
        {
            Peca = peca;
            _avaliador = modalidadeAvaliacao;
        }


        private bool NovoLanceAceito(Interessada cliente, double valor)
            => (Estado == EstadoLeilao.LeilaoEmAndamento)
            && (cliente != _ultimoCliente);

        public void RecebeLance(Interessada cliente, double valor)
        {
            if (NovoLanceAceito(cliente, valor))
            {
                _lances.Add(new Lance(cliente, valor));
                _ultimoCliente = cliente;
            }
        }

        public void IniciaPregao()
        {
            Estado = EstadoLeilao.LeilaoEmAndamento;
        }

        public void TerminaPregao()
        {
            if (Estado != EstadoLeilao.LeilaoEmAndamento)
            {
                throw new InvalidOperationException("Não é possível terminar um pregão sem que ele tenha começado. Para isso, utilize o método IniciaPregao().");
            }

            Ganhador = _avaliador.Avalia(this);

            Estado = EstadoLeilao.LeilaoFinalizado;
        }
    }
}