import { useState } from 'react';
import axios from 'axios'; 
import './CadastroTipoVeiculo.css'; 

const API_BASE_URL = process.env.REACT_APP_API_URL || "http://localhost:5285/api"; 

function CadastroTipoVeiculo() {
    // Estado para armazenar o nome do tipo de veículo digitado no formulário
    const [nomeTipo, setNomeTipo] = useState<string>(''); 
    // Estado para exibir mensagens de erro ao usuário
    const [erro, setErro] = useState<string>(''); 
    // Estado para exibir mensagens de sucesso ao usuário
    const [sucesso, setSucesso] = useState<string>(''); 

    // Função assíncrona para lidar com o envio do formulário
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault(); // Previne o comportamento padrão de recarregar a página

        setErro(''); // Limpa mensagens de erro anteriores
        setSucesso(''); // Limpa mensagens de sucesso anteriores

        // Validação básica: verifica se o nome do tipo não está vazio
        if (!nomeTipo.trim()) {
            setErro("O nome do tipo de veículo é obrigatório.");
            return; // Interrompe a execução se a validação falhar
        }

        try {
            // Envia uma requisição POST para a API para cadastrar o novo tipo de veículo
            const response = await axios.post(`${API_BASE_URL}/TipoVeiculos`, { nome: nomeTipo });

            setSucesso(`Tipo de veículo "${response.data.nome}" cadastrado com sucesso!`); // Define mensagem de sucesso
            setNomeTipo(''); // Limpa o campo do formulário após o sucesso

        } catch (error: any) {
            console.error("Erro ao cadastrar tipo de veículo:", error); // Loga o erro completo no console

            // Lógica para extrair e exibir mensagens de erro mais amigáveis do backend
            if (error.response && error.response.data && error.response.data.errors) {
                const validationErrors = Object.values(error.response.data.errors).flat();
                setErro(`Erro de validação: ${validationErrors.join(' ')}`);
            } else if (error.response && error.response.data) {
                setErro(`Erro ao cadastrar tipo de veículo: ${error.response.data}`);
            } else {
                setErro("Erro ao cadastrar tipo de veículo. Verifique sua conexão ou tente novamente.");
            }
        }
    };

    return (
        <div className="form-page-container">
            <h1 className="form-page-title">Cadastrar Tipo de Veículo</h1>

            {/* Exibe mensagem de erro se o estado 'erro' não estiver vazio */}
            {erro && (
                <div className="alert error-alert" role="alert">
                    <strong>Erro!</strong> {erro}
                </div>
            )}

            {/* Exibe mensagem de sucesso se o estado 'sucesso' não estiver vazio */}
            {sucesso && (
                <div className="alert success-alert" role="alert">
                    <strong>Sucesso!</strong> {sucesso}
                </div>
            )}

            {/* Formulário para cadastro do tipo de veículo */}
            <form onSubmit={handleSubmit} className="standard-form">
                <div className="form-group">
                    <label htmlFor="nomeTipo">Nome do Tipo de Veículo:</label>
                    <input
                        type="text"
                        id="nomeTipo"
                        value={nomeTipo}
                        onChange={(e) => setNomeTipo(e.target.value)} // Atualiza o estado 'nomeTipo' ao digitar
                        required // Campo obrigatório
                        maxLength={50} // Limite máximo de caracteres
                        minLength={3} // Limite mínimo de caracteres
                        placeholder="Ex: Carro, Moto, Caminhão"
                    />
                </div>
                <div className="form-actions">
                    <button type="submit" className="button submit-button">
                        Cadastrar
                    </button>
                </div>
            </form>
        </div>
    );
}

export default CadastroTipoVeiculo;