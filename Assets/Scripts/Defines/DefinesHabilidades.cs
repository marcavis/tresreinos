using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DefinesHabilidades
{   
    //                                          
    // public static string[] herois = new string[]  { "Zheng Xiulan", 
    //                                                 "Miao Lin", 
    //                                                 "Tao Jiang", 
    //                                                 "Liu Jingsheng", 
    //                                                 "Jiang Xun", 
    //                                                 "Guan Long"};

    //até inimigos desarmados têm uma definição de arma pra poder atacar
    public static Dictionary<string, Habilidade> habilidades = new Dictionary<string, Habilidade> {
        //public Habilidade(string nome, string descricao, int custo, float variacao, int alcanceMin, int alcanceMax,
        //      List<Vector3> areaDeEfeito, int precisao,
        //      Action<Personagem, Personagem> efeitoAtaque,
        //      bool seMesmoTime)
        {"Tiro de dispersão", new Habilidade("Tiro de dispersão", "Dispara setas contra um alvo e alvos adjacentes.", 3, 20f,
        1, 2, Habilidade.aoeCruz1, "GUI/cursor1", 0, 
        (dono, alvo) => {
            alvo.ReceberAtaque(dono.Ataque(), dono);
        }
        , false)},
        {"Reversão", new Habilidade("Reversão", "Usa o poder de ataque do inimigo contra ele próprio.", 3, 20f,
        1, 1, Habilidade.aoe0, "GUI/cursor0", 0, 
        (dono, alvo) => {
            alvo.ReceberAtaque(alvo.Ataque(), dono);
        }
        , false)},
        {"Cura", new Habilidade("Cura", "Cura, no mínimo, 5 PV da unidade alvo.", 3, 20f, 0, 2, Habilidade.aoeCruz1, "GUI/cursor1", 0,
        (dono, alvo) => {
            alvo.ReceberCura(5 + Mathf.RoundToInt(dono.nivel/4));
        }, true)},
        {"Estudo de Campo", new Habilidade("Estudo de Campo", "Torna o alvo capaz de navegar qualquer terreno rapidamente.", 3, 0f, 0, 2, Habilidade.aoe0, "GUI/cursor0", 0,
        (dono, alvo) => {
            alvo.andar = Defines.Andar(Defines.ANDAR_SUPER);
            Debug.Log(alvo.nome);
            alvo.AdicionarEfeito("Ignora Terreno");
        }, true)},
        {"Empurrão de Escudo", new Habilidade("Empurrão de Escudo", "Afasta um inimigo usando sua força e escudo; outra unidade pode ser ferida se estiver em rota de colisão.", 2, 20f,
        1, 1, Habilidade.aoe0, "GUI/cursor0", 0, 
        (dono, alvo) => {
            alvo.ReceberAtaque(1, dono);
            Vector3 deltaPos = alvo.transform.position - dono.transform.position;
            Vector3 novaPos = alvo.transform.position + deltaPos;
            Personagem naNovaPos = GameObject.Find("Gerenciador").GetComponent<GerenciadorScript>().ObjetoNoTile(novaPos);
            //alvo.TerrenoOcupavel(new Vector3Int((int) novaPos.x, (int) novaPos.y, 0))
            if(naNovaPos != null) {
                alvo.ReceberDano(alvo.mpv/10, dono);
                naNovaPos.ReceberDano(naNovaPos.mpv/10, dono);
            } else {
                if(alvo.TerrenoOcupavel(new Vector3Int((int) novaPos.x, (int) novaPos.y, 0))) {
                    alvo.transform.position += deltaPos;
                } else {
                    alvo.ReceberDano(alvo.mpv/10, dono);
                }
            }
        }
        , false)},
        {"Armageddon", new Habilidade("Armageddon", "CHEAT", 1, 35f, 1, 10, Habilidade.aoeCruz7, "GUI/cursor3", 0,
        (dono, alvo) => {
            alvo.ReceberAtaqueMagico(50, dono);
            alvo.AdicionarEfeito("Maldição");
        }, false)}
    };
}

