﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Entidades;
using Crud;
using RegrasDeNegocio;

namespace Telas
{
    public partial class TelaMenu : Form
    {
        public DataGridView gridLista;
        public Numerais.Entidade telaAtual = Numerais.Entidade.Estoque;
        public string ultimaPesquisa = null;
        public Estoque itemSelecionado = new Estoque();
       
        public TelaMenu()
        {
            InitializeComponent();
            MontaGrid(null);
        }

        private void BtnAtualizar_Click(object sender, EventArgs e)
        {
            ultimaPesquisa = null;
            MontaGrid(null);
            LimpaCampos();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            CbAlterar.Items.Clear();
            ultimaPesquisa = txtBusca.Text.ToString().Trim().ToUpper();
            MontaGrid(ultimaPesquisa);
            LimpaCampos();
        }

        private void BtnAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                Estoque itemAlterado = new Estoque();
                itemAlterado.Id = Convert.ToInt32(txtID.Text.Trim());
                itemAlterado.Nome = CbAlterar.Text.Trim();
                itemAlterado.Disponivel = Nud1.Text.Trim();
                itemAlterado.Manutencao = Nud2.Text.Trim();
                itemAlterado.Local = txtLocalA.Text.Trim();
                itemAlterado.Observacao = txtFornecedor.Text.Trim();
                itemAlterado.Data = DateTime.Now.ToShortDateString();
                CRUD.Alterar(itemAlterado);

                if (itemSelecionado.Manutencao != itemAlterado.Manutencao || itemSelecionado.Disponivel != itemAlterado.Disponivel) // só gera regsitro se as qauntidade dos items forem alteradas
                {
                    GeraRegistro(itemSelecionado);
                }
                else
                {
                    MessageBox.Show("Alteração concluida com sucesso ", "Ação concluida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                LimpaCampos();
                MontaGrid(ultimaPesquisa);
            }
            catch
            {
                MensagemExceção("Ação Não Concluida, Falha na localização do Item!!");
            }
        }

        private void BtnExclui_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(txtID.Text.Trim());
                if (MessageBox.Show("Certeza que deseja exclui o Item?", "Confirmação", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    switch ((int)telaAtual) // pega a aba atual
                    {
                        case 0:
                            Estoque itemExcluido = new Estoque();
                            itemExcluido.Id = id;
                            itemExcluido.Nome = CbAlterar.Text.Trim();
                            itemExcluido.Disponivel = Nud1.Text.Trim();
                            itemExcluido.Manutencao = Nud2.Text.Trim();
                            itemExcluido.Local = txtLocalA.Text.Trim();
                            itemExcluido.Observacao = txtFornecedor.Text.Trim();
                            itemExcluido.Data = DateTime.Now.ToShortDateString();
                            GeraRegistro(itemSelecionado);
                            CRUD.Deletar(id, telaAtual.ToString());
                            break;
                        case 1:
                            CRUD.Deletar(id, telaAtual.ToString());
                            MessageBox.Show("Item excluido com sucesso ", "Ação concluida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                    }
                    MontaGrid(ultimaPesquisa);
                    LimpaCampos();
                }
            }
            catch
            {
                MensagemExceção("Item não encontrado. Acão não concluida");
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            switch ((int)telaAtual) // pega a aba atual
            {
                case 0:
                    try
                    {
                        Estoque cadastra = new Estoque();
                        cadastra.Nome = CbAlterar.Text.Trim();
                        cadastra.Disponivel = Nud1.Text.Trim();
                        cadastra.Manutencao = Nud2.Text.Trim();
                        cadastra.Local = txtLocalA.Text.Trim();
                        cadastra.Observacao = txtFornecedor.Text.Trim();
                        cadastra.Data = DateTime.Now.ToShortDateString();
                        CRUD.Inserir(cadastra);
                        MontaGrid(ultimaPesquisa);
                        LimpaCampos();
                        MessageBox.Show("Cadastrado com sucesso", "Ação concluida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpaCampos();
                    }
                    catch
                    {
                        MensagemExceção("Erro Banco de Dado. Acão não concluida");
                    }
                    break;
                case 1:
                    try
                    {
                        Exportar.ExportarExcel(CRUD.Listar(ultimaPesquisa, telaAtual.ToString()));
                    }
                    catch 
                    {
                        MensagemExceção("Erro ao gerar excel ");
                    }
                    break;
            }
        }

        private void estoqueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((int)telaAtual != 0)
            {
                ultimaPesquisa = null;
                MontaTelaEstoque();
            }
        }

        private void registrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((int)telaAtual != 1)
            {
                ultimaPesquisa = null;
                MontaTelaRegistro();
            }
        }


        private void dgvLista_KeyUp(object sender, KeyEventArgs e)
        {
            SelecionaLinha();
        }

        private void dgvLista_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelecionaLinha();
        }

        private void SelecionaLinha()
        {
            try
            {
                switch ((int)telaAtual) // pega a aba atual
                {
                    case 0:
                        dgvLista.CurrentRow.Selected = true;
                        txtID.Text = dgvLista.CurrentRow.Cells["Id"].Value.ToString();
                        CbAlterar.Text = dgvLista.CurrentRow.Cells["Nome"].Value.ToString();
                        Nud1.Text = dgvLista.CurrentRow.Cells["Disponivel"].Value.ToString();
                        Nud2.Text = dgvLista.CurrentRow.Cells["Manutencao"].Value.ToString();
                        txtLocalA.Text = dgvLista.CurrentRow.Cells["Loca"].Value.ToString();
                        txtFornecedor.Text = dgvLista.CurrentRow.Cells["Observacao"].Value.ToString();

                        itemSelecionado.Nome = CbAlterar.Text;
                        itemSelecionado.Disponivel = Nud1.Text;
                        itemSelecionado.Manutencao = Nud2.Text;
                        itemSelecionado.Observacao = txtFornecedor.Text;
                        itemSelecionado.Local = txtLocalA.Text;
                        break;
                    case 1:
                        dgvLista.CurrentRow.Selected = true;
                        txtID.Text = dgvLista.CurrentRow.Cells["Id"].Value.ToString();
                        CbAlterar.Text = dgvLista.CurrentRow.Cells["Nome"].Value.ToString();
                        Nud1.Text = dgvLista.CurrentRow.Cells["Disponivel"].Value.ToString();
                        Nud2.Text = dgvLista.CurrentRow.Cells["Manutencao"].Value.ToString();
                        txtLocalA.Text = dgvLista.CurrentRow.Cells["Destino"].Value.ToString();
                        txtFornecedor.Text = dgvLista.CurrentRow.Cells["Observacao"].Value.ToString();
                        break;
                }
            }
            catch
            {
                MensagemExceção("Entrada de dado errada");
            }
        }

        public void MontaGrid(string NomeItem)
        {
            DataTable ListaTodosDado = CRUD.Listar(NomeItem, telaAtual.ToString());
            gridLista = dgvLista;// Atribui o elemento da tela
            this.dgvLista.DefaultCellStyle.Font = new Font("Tahoma", 11);
            gridLista.Columns.Clear();
            bool ExisteItem = true;
            switch ((int)telaAtual) // pega a aba atual
            {
                case 0:
                    if (ListaTodosDado == null)
                        ExisteItem = false;
                    else
                    {
                        gridLista.DataSource = ListaTodosDado;
                        //gridLista.AutoResizeColumns();
                        //gridLista.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; //tamnho automatico
                        gridLista.Columns["Id"].Width = 42;
                        gridLista.Columns["Nome"].Width = 240; //tamanho das colunas
                        gridLista.Columns["Disponivel"].Width = 50;
                        gridLista.Columns["Manutencao"].Width = 50;
                        gridLista.Columns["Loca"].Width = 190;
                        gridLista.Columns["Observacao"].Width = 155;
                    }
                    break;
                case 1:
                    if (ListaTodosDado == null)
                        ExisteItem = false;
                    else
                    {
                        gridLista.DataSource = ListaTodosDado;
                        gridLista.Columns["Id"].Width = 42;
                        gridLista.Columns["Nome"].Width = 240;
                        gridLista.Columns["Disponivel"].Width = 50;
                        gridLista.Columns["Manutencao"].Width = 50;
                        gridLista.Columns["Destino"].Width = 190;
                        gridLista.Columns["Observacao"].Width = 155;
                    }
                    break;
            }
            if (!ExisteItem)
            {
                MensagemExceção("Nenhum item Encontrado");
            }
        }

        public void LimpaCampos()
        {
            CbAlterar.Text = "";
            txtBusca.Clear();
            txtLocalA.Clear();
            txtFornecedor.Clear();
            Nud1.Text = "0";
            Nud2.Text = "0";
            CbAlterar.Items.Clear();
            txtID.Clear();
        }

        public void GeraRegistro(Estoque i)
        {
            TelaConfirmacao form = new TelaConfirmacao(i);
            form.ShowDialog();
        }

        private void MontaTelaEstoque()
        {
            LabelTitulo.Text = "Estoque Informatica";
            label4.Text = "Local";
            btnCadastrar.Text = "Cadastrar";
            BtnAlterar.Visible = true;
            telaAtual = Numerais.Entidade.Estoque;
            MontaGrid(null);
            txtFornecedor.ReadOnly = false;
            txtLocalA.ReadOnly = false;
            Nud1.ReadOnly = false;
            Nud2.ReadOnly = false;
        }

        private void MontaTelaRegistro()
        {
            LabelTitulo.Text = "Registros Informatica";
            label4.Text = "Destino do Item";
            btnCadastrar.Text = "Exportar";
            BtnAlterar.Visible = false;
            telaAtual = Numerais.Entidade.Registro;
            MontaGrid(null);
            txtFornecedor.ReadOnly = true;
            txtLocalA.ReadOnly = true;
            Nud1.ReadOnly = true;
            Nud2.ReadOnly = true;
        }

        private void MensagemExceção(string erro)
        {
            MessageBox.Show(erro, "Busca", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
