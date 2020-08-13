using Inlog.Service.Notificacoes;
using System.Collections.Generic;

namespace Inlog.Service.Interface
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}
