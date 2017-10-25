#Fabio Leandro Lapuinka
#Cassyo Junior         
#Equipe SAMMET  12/10/2017
#Rest API - MongoDB - BigData - Telemetria - versão 1
import unicodedata
import string
import re
import sys  
reload(sys)  
sys.setdefaultencoding('utf8')

def remove_accents(data):
    return data.encode('utf-8').strip()

#curl -d "token=value1&json=1" -X POST http://127.0.0.1:8000/DoutrinaAgil/api/find/
#curl -d "token="Dura_lex_sed_lex"a=&query=Association" -X POST https://lapuinka.pythonanywhere.com/DoutrinaAgil/dev/find/
@request.restful()
def find():
    response.view = 'generic.json'
    response.headers["Access-Control-Allow-Origin"] = '*'
    response.headers['Access-Control-Max-Age'] = 86400
    response.headers['Access-Control-Allow-Headers'] = '*'
    response.headers['Access-Control-Allow-Methods'] = '*'
    response.headers['Access-Control-Allow-Credentials'] = 'true'
    def GET2(*args,**vars):
        patterns = 'auto'
        parser = db.parse_as_rest(patterns,args,vars)
        if parser.status == 200:
            return dict(content=parser.response)
        else:
            raise HTTP(parser.status,parser.error)
    def GET(token, query):
        return POST(token,query)
    def POST(token,query):
        total  = 3 #Free Version only 3, if you need more please pay for that. The software is free, but Hardware is expensive
                   #send a email to lapuinka[arroba]gmail[ponto]com or cassyojr[arroba]gmail[ponto]com
        v= "2"
        from datetime import datetime
        agora = datetime.now().strftime('%Y-%m-%d %H:%M:%S')
        a = request.env.http_x_forwarded_for

       if str(token) != "DAWbiMVyDhNOhBOgs7vbFMhEIUrLSQ6o2FZea=": #Token versão DEMO
            return "Invalid Demo("+token+")"

        if len(str(query))<3 :
            return "Invalide Query Size, minimum 3 chars!"

        uri = "mongodb://USER:PASSWORD@SERVER.mlab.com:15942/doutrina_agil"
        client = MongoClient(uri)
        db2 = client.get_default_database()
        livro = 1
        query = str(remove_accents(unicode(query, "utf-8")))
        books = db2['books']
        doutrinas = db2['doutrinas']

        erros = "0"
        try:
            acessos = db2['acessos']
            acessos.insert({'v': v , 'ip': a , 'query': query, 'time': agora})
        except pymongo.errors.ConnectionFailure:
            erros="1"
        
        
        cursorBooks = books.find()
        
        #Coma no Café da manhã expressões regulares, você será um software muito mais inteligente!
        cursor = doutrinas.find({'texto' : {'$regex' : '.*' + query +'.*'}}).limit(total);
        livros = ""
        virgula = ""
        #Livros
        for doc in cursorBooks:
            livros  = livros + (virgula + '{"book_id":"%s" , "author":"%s", "title":"%s"}' %
                    (doc['id'], doc['author'], doc['title']))
            virgula = ","
        total = 0
        retorno = ""
        virgula = ""
        #Doutrinas
        for doc in cursor:
            #para cada bookID veja se já procurou o Link e Add na Lista de Livros...
            texto = doc['texto']
            texto = re.sub('[^a-zA-Z0-9\n\.]', ' ', texto)

            retorno  = retorno + (virgula + '{"book_id":"%s" , "page":"%s", "texto":"%s"}' %
                    (doc['book_id'], doc['page'], texto))
            virgula = ","

        if retorno == "":
            livros = ""
        try:
            estatisticas = db2['estatisticas']
            estatisticas.insert({'v': v , 'ip': a , 'query': query, 'livros': livros ,'time': agora})
            total = estatisticas.count()
        except pymongo.errors.ConnectionFailure:
            erros="1"

        est = str(total)    
        
        if retorno == "":
            return '{}'
            
        return '{"livros":['+livros+'], "doutrinas":[' + retorno + '],"q":"'+query+'", "v":"'+v+'","n":"'+agora+'","a":"'+a+'","t":"'+est+'"}'


    return dict(GET=GET, POST=POST)
