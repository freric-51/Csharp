using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;

namespace SoftwareHouse
{
	/// <summary>
	/// Summary description for Service1.
	/// </summary>


	public class Operacoes :  System.Web.Services.WebService
	{
		string Servidor, Banco, UsuarioBanco, SenhaBanco;
		string sConexaoBanco;
		private Execucao ExeClone = new Execucao();
		private SoftwareHouse.Log log = new SoftwareHouse.Log();

		//Como enumerador n�o funciona com WebService,
		//as constantes de opera��o s�o as seguintes:
		//Ricardo de Freitas - Criadas ''propriedades''
		public enum eOperacoes :int
		{
			InstalandoSDV = 1,
			InstalandoPDV = 2,
			InstalandoFidelidade = 3,
			InstalandoSitef = 4,
			InstalandoExpcli = 5,
			InstalandoBanese = 6,
			CriandoVariaveisAmbiente = 7,
			InstalandoHipercard = 8
		}

		[WebMethod(true)]
		public int OperacaoInstalandoSDV()
		{return (int)eOperacoes.InstalandoSDV;}


		[WebMethod(true)]
		public int OperacaoInstalandoPDV()
		{return (int)eOperacoes.InstalandoPDV;}


		[WebMethod(true)]
		public int OperacaoInstalandoFidelidade()
		{return (int)eOperacoes.InstalandoFidelidade;}


		[WebMethod(true)]
		public int OperacaoInstalandoSitef()
		{return (int)eOperacoes.InstalandoSitef;}


		[WebMethod(true)]
		public int OperacaoInstalandoExpcli()
		{return (int)eOperacoes.InstalandoExpcli;}


		[WebMethod(true)]
		public int OperacaoInstalandoBanese()
		{return (int)eOperacoes.InstalandoBanese;}


		[WebMethod(true)]
		public int OperacaoCriandoVariaveisAmbiente()
		{return (int)eOperacoes.CriandoVariaveisAmbiente;}


		[WebMethod(true)]
		public int OperacaoInstalandoHipercard()
		{return (int)eOperacoes.InstalandoHipercard;}


		public Operacoes()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();

			//Ricardo de Freitas
			//Parte comum :
			//conexao SQL
			Servidor = System.Configuration.ConfigurationSettings.AppSettings["ServidorSql"];
			Banco = System.Configuration.ConfigurationSettings.AppSettings["BancoSql"];
			UsuarioBanco = System.Configuration.ConfigurationSettings.AppSettings["UsuarioBanco"];
			SenhaBanco = System.Configuration.ConfigurationSettings.AppSettings["SenhaBanco"];

			if( (Servidor == "") | (Servidor == null) )
			{
				//Entendo que estou em debug e n�o sei de onde este .NET tenta l�r o arquivo
				Servidor = @"(local)\SoftwareHouse";
				Banco = "ProcessoVSAT";
				UsuarioBanco = "sa";
				SenhaBanco = "ToChangeToRealPassword";
			}
			sConexaoBanco = "Server=" + @Servidor;
			sConexaoBanco += ";Database=" + @Banco;
			sConexaoBanco += ";User Id=" + @UsuarioBanco;
			sConexaoBanco += ";Pwd=" + @SenhaBanco;
			sConexaoBanco += ";Connect Timeout=20";
		}


		#region Component Designer generated code

		//Required by the Web Services Designer
		private IContainer components = null;

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion

		[WebMethod(true)]
		public Retorno GravarInstalacaoMigracao(Execucao oExecutando)
		{
			//preparar variavel para retorno
			Retorno oRet = new Retorno();
			System.Data.SqlClient.SqlCommand oCmd;
			System.Data.SqlClient.SqlConnection oCnn;
			string sqlComando= "";
			int iRet = 0;
			oCnn = new SqlConnection("");

			try
			{
				//===== CONEXAO =====
				oCnn = new SqlConnection(@sConexaoBanco);
				oCnn.Open();
			}
			catch(Exception ex)
			{
				oCnn = null;
				oCmd = null;
				oRet.Status = 1;
				oRet.Descricao = ex.Message + " Conex�o = " + sConexaoBanco;
				return oRet;
			}

			try
			{
				//===== INSERT =====
				sqlComando =  "INSERT INTO [ProcessoVSAT].[dbo].[Execucoes]";
				sqlComando += "(";
				sqlComando += "[ID],";
				sqlComando += "[Data],";
				sqlComando += "[CodigoLoja],";
				sqlComando += "[NomeMaquina],";
				sqlComando += "[TipoInstalacao],";
				sqlComando += "[TipoEstabelecimento],";
				sqlComando += "[FuncaoComputador],";
				sqlComando += "[NumeroEstacao])";
				sqlComando += " VALUES(";
				sqlComando += "'" + Guid.NewGuid().ToString() + "',";
				sqlComando += "'" + DataToStringBanco(System.DateTime.Now) + "',";
				sqlComando += oExecutando.CodigoLoja.ToString() +",";
				sqlComando += "'" + oExecutando.NomeMaquina + "',";
				sqlComando += "'" + oExecutando.TipoInstalacao + "',";
				sqlComando += "'" + oExecutando.TipoEstabelecimento + "',";
				sqlComando += "'" + oExecutando.FuncaoComputador + "',";
				sqlComando += oExecutando.NumeroEstacao +")";

				oCmd = new SqlCommand(sqlComando,oCnn);
				iRet =(int)oCmd.ExecuteNonQuery();

				oCmd.Dispose();
				oCnn.Close();
				oCnn = null;
				oCmd = null;
			}
			catch(Exception ex)
			{
				oCnn.Close();
				oCnn = null;
				oCmd = null;
				oRet.Status = 1;
				oRet.Descricao = ex.Message + " Comando = " + sqlComando;
				return oRet;
			}

			if (iRet == 0 )
			{
				oRet.Status = 1;
				oRet.Descricao ="N�o foi registrado a instala��o.";
			}
			else
			{
				oRet.Status = 0;
				oRet.Descricao ="OK";
			}

			return oRet;
		}


		[WebMethodAttribute(true)]
		public RetornoTipoInstalacao RetornarTipoExecucao(long lLoja,string sFuncaoComputador,Int16 iEstacao)
		{
			//preparar variavel para retorno
			RetornoTipoInstalacao oRet = new RetornoTipoInstalacao();

			System.Data.SqlClient.SqlCommand oCmd;
			System.Data.SqlClient.SqlConnection oCnn;
			string sqlComando= "";
			int iRet = 0;
			oCnn = new SqlConnection("");

			try
			{
				//===== CONEXAO =====
				oCnn = new SqlConnection(@sConexaoBanco);
				oCnn.Open();
			}
			catch(Exception ex)
			{
				oCnn = null;
				oCmd = null;
				oRet.Status = 1;
				oRet.Descricao = ex.Message + " Conex�o = " + sConexaoBanco;
				oRet.TipoInstalacao="";
				return oRet;
			}

			try
			{
				//===== SELECT =====
				sqlComando =  "SELECT count(*)" ;
				sqlComando += " FROM [ProcessoVSAT].[dbo].[Execucoes]";
				sqlComando += " where [CodigoLoja] = " + lLoja ;
				sqlComando += " and [FuncaoComputador] = '" + sFuncaoComputador + "'";
				sqlComando += " and [NumeroEstacao]= " + iEstacao ;

				oCmd = new SqlCommand(sqlComando,oCnn);
				iRet =(int)oCmd.ExecuteScalar();

				oCmd.Dispose();
				oCnn.Close();
				oCnn = null;
				oCmd = null;
			}
			catch(Exception ex)
			{
				oCnn.Close();
				oCnn = null;
				oCmd = null;
				oRet.Status = 1;
				oRet.Descricao = ex.Message + " Comando = " + sqlComando;
				oRet.TipoInstalacao="";
				return oRet;
			}

			if (iRet == 0 )
			{
				oRet.Status = 0;
				oRet.Descricao ="OK ";
				oRet.TipoInstalacao = "IN";
			}
			else
			{
				oRet.Status = 0;
				oRet.Descricao ="OK";
				oRet.TipoInstalacao = "RE";
			}
			return oRet;
		}


		[WebMethodAttribute(true)]
		public string TipoEstabelecimento(bool Central_cfg, Int16 PDVs)
		{
			if (Central_cfg)
				return "LJ";//LJ = Loja
			else
			{
				if (0 == PDVs)
					return "CE";//CE = Central{Central Informal}
				else
					return "CL";//CL = Central em Loja;{Central Formal}
			}
		}


		[WebMethodAttribute(true)]
		public string FuncaoComputador(bool TemPDV, bool TemRetaguarda)
		{
			if(TemPDV)
			{
				if(TemRetaguarda)
					return "PS";//PDVArq e SuperDB
				else
					return "PD";//PDVArq
			}
			else
			{
				if(TemRetaguarda)
					return "SU";//SuperDB
				else
					return "ES";//Esta��o sem PDVArq e SuperDB
			}
		}


		[WebMethodAttribute(true)]
		public string NomeEstado(Execucao oExecutando)
		{
			ExeClone.UF = oExecutando.UF;
			return ExeClone.NomeEstado;
		}


		[WebMethod(true)]
		public string NomeMaquina(Execucao oExecutando)
		{
			//uma a uma para rodar os procedimentos ...
			ExeClone.NumeroEstacao = oExecutando.NumeroEstacao;
			ExeClone.TipoEstabelecimento = oExecutando.TipoEstabelecimento;
			ExeClone.UF = oExecutando.UF;
			ExeClone.CodigoLoja = oExecutando.CodigoLoja;
			ExeClone.FuncaoComputador = oExecutando.FuncaoComputador;

			return ExeClone.NomeMaquina;
		}


		[WebMethod(true)]
		public Retorno RegistarLog(DadosLog oExe)
		{
			//preparar variaveis
			Retorno oRet = new Retorno();
			System.Data.SqlClient.SqlCommand oCmd;
			System.Data.SqlClient.SqlConnection oCnn;
			string sqlComando ="";
			int iRet = 0;
			oCnn = new SqlConnection("");
			oCmd = new SqlCommand("",oCnn);
			try
			{
				//===== CONEXAO =====
				oCnn = new SqlConnection(@sConexaoBanco);
				oCnn.Open();
			}
			catch(Exception ex)
			{
				oCnn = null;
				oCmd = null;
				oRet.Status = 1;
				oRet.Descricao = ex.Message + " Conex�o = " + sConexaoBanco;
				return oRet;
			}

			//=========================================================
			bool PassoAlterarCodigoLoja = false;
			if( (log.CodigoLoja != oExe.CodigoLoja) | (log.Estacao != oExe.Estacao) )
				PassoAlterarCodigoLoja = true;

			log.CodigoLoja = oExe.CodigoLoja;
			log.Estacao = oExe.Estacao;
			log.TipoOperacao = oExe.TipoOperacao;
			log.Descricao = oExe.Descricao ;
			log.ID = oExe.ID;
			log.Sequencia = oExe.Sequencia;

			// 1 - verifico se ID ja existe no banco
			bool PassoNovoLog = false;
			try
			{
				sqlComando  ="select count(*)";
				sqlComando += " from [ProcessoVSAT].[dbo].[LogInstalacao]";
				sqlComando += " where ID = '" + log.ID + "'";
				oCmd = new SqlCommand(sqlComando,oCnn);
				iRet =(int)oCmd.ExecuteScalar();
				if(iRet == 0)
					PassoNovoLog = true;
				oCmd.Dispose();
			}
			catch(Exception ex)
			{
				oCmd.Dispose();
				oCnn.Close();
				oCnn = null;
				oCmd = null;
				oRet.Status = 1;
				oRet.Descricao = ex.Message + " Comando = " + sqlComando;
				return oRet;
			}

			try
			{
				//===== INSERTs =====
				if (PassoNovoLog)
				{
					sqlComando =  "INSERT INTO [ProcessoVSAT].[dbo].[LogInstalacao]";
					sqlComando += "(";
					sqlComando += "[ID],";
					sqlComando += "[Data],";
					sqlComando += "[CodigoLoja],";
					sqlComando += "[Estacao])";
					sqlComando += " VALUES(";
					sqlComando += "'" + log.ID + "',";
					sqlComando += "'" + DataToStringBanco(log.Data) + "',";
					sqlComando += log.CodigoLoja.ToString() +",";
					sqlComando += log.Estacao +")";

					oCmd = new SqlCommand(sqlComando,oCnn);
					iRet =(int)oCmd.ExecuteNonQuery();
					oCmd.Dispose();
					oCmd = null;
				}

				if(PassoAlterarCodigoLoja)
				{
					sqlComando  = "update [ProcessoVSAT].[dbo].[LogInstalacao]";
					sqlComando += " set CodigoLoja = " + log.CodigoLoja;
					sqlComando += " ,estacao = " + log.Estacao;
					sqlComando += " where ID ='" + log.ID + "'";

					oCmd = new SqlCommand(sqlComando,oCnn);
					iRet =(int)oCmd.ExecuteNonQuery();
					oCmd.Dispose();
					oCmd = null;
				}

				//detalhes ...
				sqlComando =  "INSERT INTO [ProcessoVSAT].[dbo].[DetalhesLog]";
				sqlComando += "(";
				sqlComando += "[ID],";
				sqlComando += "[IDLog],";
				sqlComando += "[TipoOperacao],";
				sqlComando += "[Descricao],";
				sqlComando += "[Sequencia])";
				sqlComando += " VALUES(";
				sqlComando += "'" + Guid.NewGuid().ToString().ToUpper() + "',";
				sqlComando += "'" + log.ID + "',";
				sqlComando += log.TipoOperacao +",";
				sqlComando += "'" + log.Descricao + "',";
				sqlComando += log.Sequencia +")";

				oCmd = new SqlCommand(sqlComando,oCnn);
				iRet =(int)oCmd.ExecuteNonQuery();
				oCmd.Dispose();
				oCmd = null;
			}
			catch(Exception ex)
			{
				oCnn.Close();
				oCnn = null;
				oCmd = null;
				oRet.Status = 1;
				oRet.Descricao = ex.Message + " Comando = " + sqlComando;
				return oRet;
			}

			oRet.Status = 0;
			oRet.Descricao ="OK";
			return oRet;
		}


		[WebMethodAttribute(true)]
		public Retorno GravarLogTEF(LogMigracaoTEF log, BandeirasLogMigracaoTEF band)
		{
			//Ricardo de Freitas 10jan2006
			//preparar variavel para retorno
			Retorno oRet = new Retorno();
			System.Data.SqlClient.SqlCommand oCmd;
			System.Data.SqlClient.SqlConnection oCnn;
			string sqlComando= "";
			int iRet = 0;
			oCnn = new SqlConnection("");
			oCmd = new SqlCommand("",oCnn);
			try
			{
				//===== CONEXAO =====
				oCnn = new SqlConnection(@sConexaoBanco);
				oCnn.Open();
			}
			catch(Exception ex)
			{
				oCnn = null;
				oCmd = null;
				oRet.Status = 1;
				oRet.Descricao = ex.Message + " Conex�o = " + sConexaoBanco;
				return oRet;
			}

			// 1 - verifico se ID ja existe no banco
			bool PassoNovoLog = false;
			try
			{
				sqlComando  ="select count(*)";
				sqlComando += " from [ProcessoVSAT].[dbo].[LogMigracaoTEF]";
				sqlComando += " where ID = '" + log.ID + "'";
				oCmd = new SqlCommand(sqlComando,oCnn);
				iRet =(int)oCmd.ExecuteScalar();
				if(iRet == 0)
					PassoNovoLog = true;
				oCmd.Dispose();
			}
			catch(Exception ex)
			{
				oCmd.Dispose();
				oCnn.Close();
				oCnn = null;
				oCmd = null;
				oRet.Status = 1;
				oRet.Descricao = ex.Message + " Comando = " + sqlComando;
				return oRet;
			}

			try
			{
				//===== INSERTs =====
				if (PassoNovoLog)
				{
					sqlComando =  "INSERT INTO [ProcessoVSAT].[dbo].[LogMigracaoTEF]";
					sqlComando += "(";
					sqlComando += "[ID],";
					sqlComando += "[DataRegistro],";
					sqlComando += "[DataImpressao],";
					sqlComando += "[DataMovimento],";
					sqlComando += "[CodigoLoja])";
					sqlComando += " VALUES(";
					sqlComando += "'" + log.ID + "',";
					sqlComando += "'" + DataToStringBanco(log.Data) + "',";
					sqlComando += "'" + DataToStringBanco(log.DataImpressao) + "',";
					sqlComando += "'" + DataToStringBanco(log.DataMovimento) + "',";
					sqlComando += log.CodigoLoja.ToString() +")";

					oCmd = new SqlCommand(sqlComando,oCnn);
					iRet =(int)oCmd.ExecuteNonQuery();
					oCmd.Dispose();
					oCmd = null;
				}

				sqlComando =  "INSERT INTO [ProcessoVSAT].[dbo].[BandeirasLogMigracaoTef]";
				sqlComando += "(";
				sqlComando += "[ID],";
				sqlComando += "[IdLogMigracaoTef],";
				sqlComando += "[TipoBandeira],";
				sqlComando += "[Estado],";
				sqlComando += "[CodigoTransacao],";
				sqlComando += "[CodigoResposta])";
				sqlComando += " VALUES(";
				sqlComando += "'" + band.ID + "',";
				sqlComando += "'" + band.idLogMigracaoTef + "',";
				sqlComando += band.TipoBandeira.ToString() +",";
				sqlComando += "'" + band.Estado + "',";
				sqlComando += "'" + band.CodigoTransacao + "',";
				sqlComando += "'" + band.CodigoResposta +"')";

				//apenas gravar a abertura de terminal
				//if (band.CodigoTransacao == "ABERTERM")
				//{
					oCmd = new SqlCommand(sqlComando,oCnn);
					iRet =(int)oCmd.ExecuteNonQuery();
				//}
				//else
				//	iRet=1;

				oCmd.Dispose();
				oCnn.Close();
				oCnn = null;
				oCmd = null;
			}
			catch(Exception ex)
			{
				oCnn.Close();
				oCnn = null;
				oCmd = null;
				oRet.Status = 1;
				oRet.Descricao = ex.Message + " Comando = " + sqlComando;
				return oRet;
			}

			if (iRet == 0 )
			{
				oRet.Status = 1;
				oRet.Descricao ="N�o foi registrado a instala��o.";
			}
			else
			{
				oRet.Status = 0;
				oRet.Descricao ="OK";
			}

			return oRet;
		}

		private string DataToStringBanco(DateTime dt)
		{
			string sDT;
			sDT = dt.Year.ToString();
			sDT += "-" + dt.Month.ToString();
			sDT += "-" + dt.Day.ToString();
			sDT += " " + dt.Hour.ToString();
			sDT += ":" + dt.Minute.ToString();
			sDT += ":" + dt.Second.ToString();
			return sDT;
		}
	}

}
